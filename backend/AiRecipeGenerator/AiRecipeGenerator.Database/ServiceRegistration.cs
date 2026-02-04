using AiRecipeGenerator.Database.Factories;
using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Repositories;

using Microsoft.Extensions.DependencyInjection;

namespace AiRecipeGenerator.Database;

public static class ServiceRegistration
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new SqliteConnectionFactory(connectionString));
        services.AddSingleton<IApiKeyRepository, ApiKeyRepository>();
        services.AddSingleton<IRecipeRepository, RecipeRepository>();
        services.AddSingleton<IIngredientCategoryRepository, IngredientCategoryRepository>();
        services.AddSingleton<IIngredientRepository, IngredientRepository>();

        return services;
    }
}
