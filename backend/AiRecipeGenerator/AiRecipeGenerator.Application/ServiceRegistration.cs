using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Application.Services;

using Microsoft.Extensions.DependencyInjection;

namespace AiRecipeGenerator.Application;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<IApiKeyService, ApiKeyService>();
        services.AddSingleton<IRecipeService, RecipeService>();
        services.AddSingleton<IIngredientCategoryService, IngredientCategoryService>();
        services.AddSingleton<IIngredientService, IngredientService>();

        return services;
    }
}
