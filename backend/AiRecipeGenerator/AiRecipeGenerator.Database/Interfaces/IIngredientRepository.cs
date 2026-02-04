using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Models.Repository;

namespace AiRecipeGenerator.Database.Interfaces;

public interface IIngredientRepository
{
    Task<PaginatedResultModel<IngredientRepositoryModel>> GetAsync(GetIngredientsQueryModel queryModel);
    Task UpdateAsync(UpdateIngredientCommandModel commandModel);
    Task AddAsync(AddIngredientCommandModel commandModel);
    Task DeleteAsync(int id);
    Task<IEnumerable<(IngredientRepositoryModel Ingredient, string CategoryName)>> GetAllAsync();
}
