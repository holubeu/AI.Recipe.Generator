using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Dtos;

namespace AiRecipeGenerator.API.Mappings.Responses;

public static class IngredientResponseMappings
{
    public static PaginatedResponseModel<GetIngredientResponseModel> ToPaginatedGetIngredientResponseModel(this PaginatedResultDto<IngredientDto> dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Items = dto.Items.ToGetIngredientResponseModels(),
            Total = dto.Total
        };
    }

    private static IEnumerable<GetIngredientResponseModel> ToGetIngredientResponseModels(this IEnumerable<IngredientDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);

        return dtos.Select(x => x.ToGetIngredientResponseModel());
    }

    private static GetIngredientResponseModel ToGetIngredientResponseModel(this IngredientDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id,
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            IsVisibleOnCard = dto.IsVisibleOnCard,
            ImagePath = dto.ImagePath,
            CreatedOn = dto.CreatedOn,
            UpdatedOn = dto.UpdatedOn
        };
    }
}
