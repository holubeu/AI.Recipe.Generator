using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Repository;

namespace AiRecipeGenerator.Application.Mappings;

public static class IngredientCategoryMappings
{
    public static PaginatedResultDto<IngredientCategoryDto> ToIngredientCategoryDtosPaginated(this PaginatedResultModel<IngredientCategoryRepositoryModel> paginatedResult)
    {
        ArgumentNullException.ThrowIfNull(paginatedResult);

        return new()
        {
            Items = paginatedResult.Items.ToIngredientCategoryDtos(),
            Total = paginatedResult.Total
        };
    }

    private static IEnumerable<IngredientCategoryDto> ToIngredientCategoryDtos(this IEnumerable<IngredientCategoryRepositoryModel> repositoryModels)
    {
        ArgumentNullException.ThrowIfNull(repositoryModels);

        return repositoryModels.Select(x => x.ToIngredientCategoryDto());
    }

    private static IngredientCategoryDto ToIngredientCategoryDto(this IngredientCategoryRepositoryModel repositoryModel)
    {
        ArgumentNullException.ThrowIfNull(repositoryModel);

        return new()
        {
            Id = repositoryModel.Id,
            Name = repositoryModel.Name,
            CreatedOn = repositoryModel.CreatedOn,
            UpdatedOn = repositoryModel.UpdatedOn
        };
    }
}
