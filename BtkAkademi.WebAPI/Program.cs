using BtkAkademi.Presentation;
using BtkAkademi.Services.Contracts;
using BtkAkademi.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using NLog;

var builder = WebApplication.CreateBuilder(args);

//add nlog configration on program
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.AddControllers(config =>
{
    config.RespectBrowserAcceptHeader = true;
    config.ReturnHttpNotAcceptable = true;
})
    .AddXmlDataContractSerializerFormatters()
    .AddApplicationPart(typeof(AssemblyRefference).Assembly)
    .AddNewtonsoftJson();

//for the validations
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//add database connection
builder.Services.ConfigureSqlContext(builder.Configuration);
//add repository managaer injection
builder.Services.ConfigureRepositoryManager();
//add service manager injection via ServiceExtensions class
builder.Services.ConfigureServiceManager();
//inject logger service
builder.Services.ConfigureLoggerServicer();
//add aoutomapper
builder.Services.AddAutoMapper(typeof(Program));
//add action filters
builder.Services.ConfigureActionFilters();
//configure Cors
builder.Services.ConfigureCors();
//configure data shaper
builder.Services.ConfigureDataShaper();

var app = builder.Build();
//logger service
var loggerService = app.Services.GetRequiredService<ILoggerService>();
//Configure exception handler
app.ConfigureExceptionHandler(loggerService);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();
