using chatlaapp.Backend.Filters;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using chatlaapp.Backend.Data;

var builder = WebApplication.CreateBuilder(args);

// Database configuration
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (string.IsNullOrEmpty(databaseUrl))
{
    throw new InvalidOperationException("DATABASE_URL environment variable is not set");
}

// Parse database connection string
var databaseUri = new Uri(databaseUrl);
var userInfo = databaseUri.UserInfo.Split(':');
var connectionBuilder = new NpgsqlConnectionStringBuilder
{
    Host = databaseUri.Host,
    Port = databaseUri.Port,
    Username = userInfo[0],
    Password = userInfo[1],
    Database = databaseUri.LocalPath.TrimStart('/'),
    SslMode = SslMode.Require
};

// Add DbContext to services
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionBuilder.ToString()));

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelFilter>();
    options.Filters.Add<ImageValidationFilter>();
    options.Filters.Add<UserActionFilter>();
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatLA API", Version = "v1" });
    
    // Add support for file uploads in Swagger
    c.OperationFilter<FileUploadOperationFilter>();
});

// Add CORS support
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Default Vite dev server port
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowFrontend");

// Add authorization middleware
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Remove or comment out the weather forecast example code
// var summaries = new[] { ... };
// app.MapGet("/weatherforecast", () => { ... });

app.Run();

// Remove or comment out the WeatherForecast record
// record WeatherForecast(...) { ... }

