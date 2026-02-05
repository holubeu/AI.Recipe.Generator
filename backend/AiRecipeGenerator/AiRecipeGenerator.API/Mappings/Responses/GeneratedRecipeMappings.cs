using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.API.Mappings.Responses;

public static class GeneratedRecipeMappings
{
    public static GenerateRecipeQueryModel ToGenerateRecipeQueryModel(this GenerateRecipeRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        return new()
        {
            Ingredients = requestModel.Ingredients
        };
    }

    public static GeneratedRecipeResponseModel ToGeneratedRecipeResponseModel(this GeneratedRecipeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            RecipeFound = dto.RecipeFound,
            Message = dto.Message,
            Recipe = dto.Recipe != null ? dto.Recipe.ToRecipeContentResponseModel() : null
        };
    }

    private static RecipeContentResponseModel ToRecipeContentResponseModel(this RecipeContentDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Name = dto.Name,
            Description = dto.Description,
            DishType = dto.DishType,
            Steps = dto.Steps,
            CookingTime = dto.CookingTime != null ? dto.CookingTime.ToCookingTimeResponseModel() : null
        };
    }

    private static CookingTimeResponseModel ToCookingTimeResponseModel(this CookingTimeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            From = dto.From,
            To = dto.To
        };
    }
}
