using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Dtos;

namespace AiRecipeGenerator.API.Mappings.Responses;

public static class GetAllIngredientsResponseMappings
{
    public static IEnumerable<GetAllIngredientsResponseModel> ToGetAllIngredientsResponseModels(this IEnumerable<GetAllIngredientsDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);

        return dtos.Select(x => x.ToGetAllIngredientsResponseModel());
    }

    private static GetAllIngredientsResponseModel ToGetAllIngredientsResponseModel(this GetAllIngredientsDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Category = dto.Category,
            Ingredients = dto.Ingredients.ToIngredientByGroupResponseModels()
        };
    }

    private static IEnumerable<IngredientByGroupResponseModel> ToIngredientByGroupResponseModels(this IEnumerable<IngredientByGroupDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);

        return dtos.Select(x => x.ToIngredientByGroupResponseModel());
    }

    private static IngredientByGroupResponseModel ToIngredientByGroupResponseModel(this IngredientByGroupDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Name = dto.Name,
            IsVisibleOnCard = dto.IsVisibleOnCard,
            ImagePath = dto.ImagePath
        };
    }
}
