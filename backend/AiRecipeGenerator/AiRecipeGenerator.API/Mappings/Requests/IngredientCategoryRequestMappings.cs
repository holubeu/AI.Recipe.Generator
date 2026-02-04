using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.API.Mappings.Requests;

public static class IngredientCategoryRequestMappings
{
    public static GetIngredientCategoriesQueryModel ToGetIngredientCategoriesQueryModel(this GetIngredientCategoriesRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        return new()
        {
            Skip = requestModel.Skip,
            Take = requestModel.Take
        };
    }

    public static AddIngredientCategoryCommandModel ToAddIngredientCategoryCommandModel(this AddIngredientCategoryRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        return new()
        {
            Name = requestModel.Name
        };
    }

    public static UpdateIngredientCategoryCommandModel ToUpdateIngredientCategoryCommandModel(this UpdateIngredientCategoryRequestModel requestModel)
    {
        ArgumentNullException.ThrowIfNull(requestModel);

        return new()
        {
            Id = requestModel.Id,
            Name = requestModel.Name
        };
    }
}
