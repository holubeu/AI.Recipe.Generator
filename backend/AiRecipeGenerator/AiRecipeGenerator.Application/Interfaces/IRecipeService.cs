using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.Application.Interfaces;

public interface IRecipeService
{
    Task<PaginatedResultDto<RecipeDto>> GetAsync(GetRecipesQueryModel queryModel);
    Task UpdateAsync(UpdateRecipeCommandModel commandModel);
    Task AddAsync(AddRecipeCommandModel commandModel);
    Task DeleteAsync(int id);
}
