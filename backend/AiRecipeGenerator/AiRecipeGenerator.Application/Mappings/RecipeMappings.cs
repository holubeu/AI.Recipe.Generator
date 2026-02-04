using System.Text.Json;

using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Repository;

namespace AiRecipeGenerator.Application.Mappings;

public static class RecipeMappings
{
    public static PaginatedResultDto<RecipeDto> ToRecipeDtosPaginated(this PaginatedResultModel<RecipeRepositoryModel> paginatedResult)
    {
        ArgumentNullException.ThrowIfNull(paginatedResult);

        return new()
        {
            Items = paginatedResult.Items.ToRecipeDtos(),
            Total = paginatedResult.Total
        };
    }

    private static IEnumerable<RecipeDto> ToRecipeDtos(this IEnumerable<RecipeRepositoryModel> repositoryModels)
    {
        ArgumentNullException.ThrowIfNull(repositoryModels);

        return repositoryModels.Select(x => x.ToRecipeDto());
    }

    private static RecipeDto ToRecipeDto(this RecipeRepositoryModel repositoryModel)
    {
        ArgumentNullException.ThrowIfNull(repositoryModel);

        string[] steps;
        if (string.IsNullOrWhiteSpace(repositoryModel.Steps))
        {
            throw new InvalidOperationException($"Recipe steps JSON is null or empty: {repositoryModel.Steps}");
        }
        else
        {
            try
            {
                steps = JsonSerializer.Deserialize<string[]>(repositoryModel.Steps)
                    ?? throw new InvalidOperationException($"Recipe steps JSON did not deserialize to a valid string array: {repositoryModel.Steps}");
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException($"Invalid JSON format in recipe steps: {repositoryModel.Steps}", ex);
            }
        }

        return new()
        {
            Id = repositoryModel.Id,
            Name = repositoryModel.Name,
            Description = repositoryModel.Description,
            DishType = repositoryModel.DishType,
            CookingTimeFrom = repositoryModel.CookingTimeFrom,
            CookingTimeTo = repositoryModel.CookingTimeTo,
            Steps = steps,
            CreatedOn = repositoryModel.CreatedOn,
            UpdatedOn = repositoryModel.UpdatedOn
        };
    }
}
