using AspNetCoreRateLimit;
using BtkAkademi.Presentation;
using BtkAkademi.Services;
using BtkAkademi.Services.Contracts;
using BtkAkademi.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NLog;

var builder = WebApplication.CreateBuilder(args);

//add nlog configration on program
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
        config.CacheProfiles.Add("5mins", new CacheProfile() {Duration = 300});
    })
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(AssemblyRefference).Assembly)
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

//for the validations
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();

//add database connection
builder.Services.ConfigureSqlContext(builder.Configuration);
//Add Services
builder.Services.ConfigureRepositoryManager();
builder.Services.RegisterRepositories();
builder.Services.ConfigureServiceManager();
builder.Services.RegisterServices();
builder.Services.ConfigureLoggerServicer();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ConfigureActionFilters();
builder.Services.ConfigureCors();
builder.Services.ConfigureDataShaper();
builder.Services.ConfigureCustomMediaTypes();
builder.Services.AddScoped<IBookLinks, BookLinks>();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureResponseCaching();
builder.Services.ConfigureHttpCacheHeaders();
builder.Services.AddMemoryCache();
builder.Services.ConfigureRateLimitOptions();
builder.Services.AddHttpContextAccessor();

//Identity Configurations
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(builder.Configuration);



var app = builder.Build();
//logger service
var loggerService = app.Services.GetRequiredService<ILoggerService>();
//Configure exception handler
app.ConfigureExceptionHandler(loggerService);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(s =>
    {
        s.SwaggerEndpoint("/swagger/v1/swagger.json" , "Btk Akademi V1");
        s.SwaggerEndpoint("/swagger/v2/swagger.json" , "Btk Akademi V2");
    });
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseIpRateLimiting();
app.UseCors("CorsPolicy");
app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
