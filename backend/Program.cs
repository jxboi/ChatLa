using chatlaapp.Backend.Filters;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

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

