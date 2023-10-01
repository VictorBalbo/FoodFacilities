using System.Reflection;
using FoodFacilities.Data;
using FoodFacilitiesAPI;
using FoodFacilitiesAPI.Configurations;
using FoodFacilitiesAPI.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Register Dependency Injections
var configurations = builder.Configuration.GetSection("ApplicationConfigurations").Get<Configuration>()!;
builder.Services.RegisterApiDependencies(configurations);
builder.Services.RegisterDataDependencies(configurations.DatabaseName);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "FoodFacilities.API",
        Description = "An ASP.NET Core Web API for managing Food Facility Permits from San Francisco",
        Contact = new OpenApiContact
        {
            Name = "Victor Balbo",
            Email = "victor@victorbalbo.com"
        },
    });
    
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();