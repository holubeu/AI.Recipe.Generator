using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Dtos;

namespace AiRecipeGenerator.API.Mappings.Responses;

public static class IngredientCategoryResponseMappings
{
    public static PaginatedResponseModel<GetIngredientCategoryResponseModel> ToPaginatedGetIngredientCategoryResponseModel(this PaginatedResultDto<IngredientCategoryDto> dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Items = dto.Items.ToGetIngredientCategoryResponseModels(),
            Total = dto.Total
        };
    }

    private static IEnumerable<GetIngredientCategoryResponseModel> ToGetIngredientCategoryResponseModels(this IEnumerable<IngredientCategoryDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);

        return dtos.Select(x => x.ToGetIngredientCategoryResponseModel());
    }

    private static GetIngredientCategoryResponseModel ToGetIngredientCategoryResponseModel(this IngredientCategoryDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id,
            Name = dto.Name,
            CreatedOn = dto.CreatedOn,
            UpdatedOn = dto.UpdatedOn
        };
    }
}
