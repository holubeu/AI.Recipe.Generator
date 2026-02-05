using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.API.Mappings.Requests;

public static class RecipeRequestMappings
{
    public static GetRecipesQueryModel ToGetRecipesQueryModel(this GetRecipesRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);
        return new()
        {
            Name = requestModel.Name,
            DishType = requestModel.DishType,
            MaxCookingTime = requestModel.MaxCookingTime,
            Skip = requestModel.Skip,
            Take = requestModel.Take
        };
    }

    public static AddRecipeCommandModel ToAddRecipeCommandModel(this AddRecipeRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);
        return new()
        {
            Name = requestModel.Name,
            Description = requestModel.Description,
            DishType = requestModel.DishType,
            CookingTimeFrom = requestModel.CookingTimeFrom,
            CookingTimeTo = requestModel.CookingTimeTo,
            Steps = System.Text.Json.JsonSerializer.Serialize(requestModel.Steps)
        };
    }

    public static UpdateRecipeCommandModel ToUpdateRecipeCommandModel(this UpdateRecipeRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);
        return new()
        {
            Id = requestModel.Id,
            Name = requestModel.Name,
            Description = requestModel.Description,
            DishType = requestModel.DishType,
            CookingTimeFrom = requestModel.CookingTimeFrom,
            CookingTimeTo = requestModel.CookingTimeTo,
            Steps = System.Text.Json.JsonSerializer.Serialize(requestModel.Steps)
        };
    }
}
