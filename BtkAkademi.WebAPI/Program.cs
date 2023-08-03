using BtkAkademi.Repositories.EFCore;
using BtkAkademi.WebAPI.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
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
