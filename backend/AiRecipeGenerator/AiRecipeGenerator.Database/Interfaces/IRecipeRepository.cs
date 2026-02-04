using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Models.Repository;

namespace AiRecipeGenerator.Database.Interfaces;

public interface IRecipeRepository
{
    Task<PaginatedResultModel<RecipeRepositoryModel>> GetAsync(GetRecipesQueryModel queryModel);
    Task UpdateAsync(UpdateRecipeCommandModel commandModel);
    Task AddAsync(AddRecipeCommandModel commandModel);
    Task DeleteAsync(int id);
}
