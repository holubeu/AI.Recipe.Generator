using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.API.Mappings.Requests;

public static class IngredientRequestMappings
{
    public static GetIngredientsQueryModel ToGetIngredientsQueryModel(this GetIngredientsRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        return new()
        {
            Name = requestModel.Name,
            CategoryId = requestModel.CategoryId,
            Skip = requestModel.Skip,
            Take = requestModel.Take
        };
    }

    public static AddIngredientCommandModel ToAddIngredientCommandModel(this AddIngredientRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        return new()
        {
            Name = requestModel.Name,
            CategoryId = requestModel.CategoryId,
            IsVisibleOnCard = requestModel.IsVisibleOnCard,
            ImagePath = requestModel.ImagePath
        };
    }

    public static UpdateIngredientCommandModel ToUpdateIngredientCommandModel(this UpdateIngredientRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        return new()
        {
            Id = requestModel.Id,
            Name = requestModel.Name,
            CategoryId = requestModel.CategoryId,
            IsVisibleOnCard = requestModel.IsVisibleOnCard,
            ImagePath = requestModel.ImagePath
        };
    }
}
