using AiRecipeGenerator.API.Authentication;
using AiRecipeGenerator.API.Middleware;
using AiRecipeGenerator.Application;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Application.Services;
using AiRecipeGenerator.Database;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add database services including repositories and connection factory
builder.Services.AddDatabaseServices(connectionString);

// Add application services
builder.Services.AddApplicationServices();

// Add database initialization service
builder.Services.AddSingleton<IDatabaseInitializationService>(
    new DatabaseInitializationService(connectionString));

var app = builder.Build();

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

// Add role middleware to extract role from headers
app.UseMiddleware<RoleMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
