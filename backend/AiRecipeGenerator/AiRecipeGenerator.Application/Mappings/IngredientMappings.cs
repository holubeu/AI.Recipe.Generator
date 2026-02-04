using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Repository;

namespace AiRecipeGenerator.Application.Mappings;

public static class IngredientMappings
{
    public static PaginatedResultDto<IngredientDto> ToIngredientDtosPaginated(this PaginatedResultModel<IngredientRepositoryModel> paginatedResult)
    {
        ArgumentNullException.ThrowIfNull(paginatedResult);

        return new()
        {
            Items = paginatedResult.Items.ToIngredientDtos(),
            Total = paginatedResult.Total
        };
    }

    private static IEnumerable<IngredientDto> ToIngredientDtos(this IEnumerable<IngredientRepositoryModel> repositoryModels)
    {
        ArgumentNullException.ThrowIfNull(repositoryModels);

        return repositoryModels.Select(x => x.ToIngredientDto());
    }

    private static IngredientDto ToIngredientDto(this IngredientRepositoryModel repositoryModel)
    {
        ArgumentNullException.ThrowIfNull(repositoryModel);

        return new()
        {
            Id = repositoryModel.Id,
            Name = repositoryModel.Name,
            CategoryId = repositoryModel.CategoryId,
            IsVisibleOnCard = repositoryModel.IsVisibleOnCard,
            CreatedOn = repositoryModel.CreatedOn,
            UpdatedOn = repositoryModel.UpdatedOn,
            ImagePath = repositoryModel.ImagePath
        };
    }
}
