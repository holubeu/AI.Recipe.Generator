using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models.Repository;

namespace AiRecipeGenerator.Application.Mappings;

public static class GetAllIngredientsMappings
{
    public static IEnumerable<GetAllIngredientsDto> ToGetAllIngredientsDtos(this IEnumerable<IGrouping<string, (IngredientRepositoryModel Ingredient, string CategoryName)>> groupedIngredients)
    {
        ArgumentNullException.ThrowIfNull(groupedIngredients);

        return groupedIngredients.Select(x => x.ToGetAllIngredientsDto());
    }

    private static GetAllIngredientsDto ToGetAllIngredientsDto(this IGrouping<string, (IngredientRepositoryModel Ingredient, string CategoryName)> group)
    {
        ArgumentNullException.ThrowIfNull(group);

        return new()
        {
            Category = group.Key,
            Ingredients = group.Select(x => x.Ingredient).ToIngredientByGroupDtos()
        };
    }

    private static IEnumerable<IngredientByGroupDto> ToIngredientByGroupDtos(this IEnumerable<IngredientRepositoryModel> repositoryModels)
    {
        ArgumentNullException.ThrowIfNull(repositoryModels);

        return repositoryModels.Select(x => x.ToIngredientByGroupDto());
    }

    private static IngredientByGroupDto ToIngredientByGroupDto(this IngredientRepositoryModel repositoryModel)
    {
        ArgumentNullException.ThrowIfNull(repositoryModel);

        return new()
        {
            Name = repositoryModel.Name,
            IsVisibleOnCard = repositoryModel.IsVisibleOnCard,
            ImagePath = repositoryModel.ImagePath
        };
    }
}
