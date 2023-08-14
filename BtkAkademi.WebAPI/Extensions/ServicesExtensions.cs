
using System.Text;
using AspNetCoreRateLimit;
using BtkAkademi.Entities.Dtos;
using BtkAkademi.Entities.Models;
using BtkAkademi.Presentation.ActionFilters;
using BtkAkademi.Repositories.Contracts;
using BtkAkademi.Repositories.EFCore;
using BtkAkademi.Services;
using BtkAkademi.Services.Contracts;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BtkAkademi.WebAPI.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services,
            IConfiguration configuration) => services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("BtkDatabase")));
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();
        public static void ConfigureLoggerServicer(this IServiceCollection services) =>
            services.AddSingleton<ILoggerService, LoggerManager>();
        public static void ConfigureActionFilters(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();
            services.AddScoped<ValidateMediaTypesAttribute>();
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("X-Pagination")
                );
            });
        }

        public static void ConfigureDataShaper(this IServiceCollection services) =>
            services.AddScoped<IDataShaper<BookDto>, DataShaper<BookDto>>();

        public static void ConfigureCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var systemTextJsonFormatter = config
                .OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

                if (systemTextJsonFormatter is not null)
                {
                    systemTextJsonFormatter.SupportedMediaTypes
                        .Add("application/vnd.btkakademi.hateoas+json");
                    systemTextJsonFormatter.SupportedMediaTypes
                        .Add("application/vnd.btkakademi.apiroot+json");
                }




                var xmlOutputFormatter = config
                .OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

                if (xmlOutputFormatter is not null)
                {
                    xmlOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.btkakademi.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.btkakademi.apiroot+xml");
                }



            });
        }

        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        public static void ConfigureResponseCaching(this IServiceCollection services) =>
            services.AddResponseCaching();

        public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
        services.AddHttpCacheHeaders(expirationOpt =>
        {
            expirationOpt.MaxAge = 70;
            expirationOpt.CacheLocation = CacheLocation.Public;
        });

        public static void ConfigureRateLimitOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>()
            {
                new RateLimitRule()
                {
                    Endpoint = "*",
                    Limit = 15,
                    Period = "1m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });

            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<RepositoryContext>()
                .AddDefaultTokenProviders();
            
        }

        public static void ConfigureJWT(this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = configuration.GetSection("JwtSettings");
            var secretKey = settings["secretKey"];

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = settings["validIssuer"],
                    ValidAudience = settings["validAuidence"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                    
                };
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", 
                    new OpenApiInfo
                    {
                        Title = "Btk Akademi", 
                        Version = "v1",
                        Description = "Btk Akademi Web API",
                        TermsOfService = new Uri("https://github.com/tolgaozkann"),
                        Contact = new OpenApiContact
                        {
                            Name = "Tolga Şükrü Özkan",
                            Email = "tolgasukruozkan@gmail.com",
                            Url = new Uri("https://github.com/tolgaozkann")
                        }
                    });
                s.SwaggerDoc("v2", new OpenApiInfo{ Title = "Btk Akademi", Version = "v2"});

                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Place to add JWT with Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                s.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer"
                        },
                        new List<string>()
                    }
                });
            });

            
        }
    }
}
