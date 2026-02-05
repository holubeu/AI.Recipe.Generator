using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Application.Models.OpenRouter;

namespace AiRecipeGenerator.Application.Mappings;

public static class OpenRouterRecipeMappings
{
    public static GeneratedRecipeDto ToGeneratedRecipeDto(this RecipeContent recipeContent)
    {
        ArgumentNullException.ThrowIfNull(recipeContent);

        return new GeneratedRecipeDto
        {
            RecipeFound = recipeContent.RecipeFound,
            Message = recipeContent.Message,
            Recipe = recipeContent.Recipe != null ? recipeContent.Recipe.ToRecipeContentDto() : null
        };
    }

    private static RecipeContentDto ToRecipeContentDto(this RecipeInfo recipeInfo)
    {
        ArgumentNullException.ThrowIfNull(recipeInfo);

        return new RecipeContentDto
        {
            Name = recipeInfo.Name,
            Description = recipeInfo.Description,
            DishType = recipeInfo.DishType,
            Steps = recipeInfo.Steps,
            CookingTime = recipeInfo.CookingTime != null ? recipeInfo.CookingTime.ToCookingTimeDto() : null
        };
    }

    private static CookingTimeDto ToCookingTimeDto(this CookingTime cookingTime)
    {
        ArgumentNullException.ThrowIfNull(cookingTime);

        return new CookingTimeDto
        {
            From = cookingTime.From,
            To = cookingTime.To
        };
    }
}
