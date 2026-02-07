using AiRecipeGenerator.API.Middleware;
using AiRecipeGenerator.Application;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Application.Services;
using AiRecipeGenerator.Database;

using Microsoft.Extensions.FileProviders;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .AllowAnyHeader();
    });
});

// Add database services including repositories and connection factory
builder.Services.AddDatabaseServices(connectionString);

// Add application services
builder.Services.AddApplicationServices();

// Add database initialization service
builder.Services.AddSingleton<IDatabaseInitializationService>(
    new DatabaseInitializationService(connectionString));

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "Images")),
    RequestPath = "/images"
});

// Initialize database at startup
using (var scope = app.Services.CreateScope())
{
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializationService>();
    await databaseInitializer.InitializeDatabaseAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Add error handling middleware (must be before other middleware that might throw)
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

// Enable CORS
app.UseCors();

// Add role middleware to extract role from headers
app.UseMiddleware<RoleMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
