using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Dtos;

namespace AiRecipeGenerator.API.Mappings.Responses;

public static class RecipeResponseMappings
{
    public static PaginatedResponseModel<GetRecipeResponseModel> ToPaginatedGetRecipeResponseModel(this PaginatedResultDto<RecipeDto> dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Items = dto.Items.ToGetRecipeResponseModels(),
            Total = dto.Total
        };
    }

    private static IEnumerable<GetRecipeResponseModel> ToGetRecipeResponseModels(this IEnumerable<RecipeDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos);

        return dtos.Select(x => x.ToGetRecipeResponseModel());
    }

    private static GetRecipeResponseModel ToGetRecipeResponseModel(this RecipeDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        return new()
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            DishType = dto.DishType,
            CookingTimeFrom = dto.CookingTimeFrom,
            CookingTimeTo = dto.CookingTimeTo,
            Steps = dto.Steps,
            CreatedOn = dto.CreatedOn,
            UpdatedOn = dto.UpdatedOn
        };
    }
}
