using BtkAkademi.Presentation;
using BtkAkademi.Repositories.EFCore;
using BtkAkademi.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;
using NLog;

var builder = WebApplication.CreateBuilder(args);

//add nlog configration on program
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AssemblyRefference).Assembly)
    .AddNewtonsoftJson();
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
