using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;
using AiRecipeGenerator.Database.Models.Repository;

namespace AiRecipeGenerator.Database.Interfaces;

public interface IIngredientCategoryRepository
{
    Task<PaginatedResultModel<IngredientCategoryRepositoryModel>> GetAllAsync(GetIngredientCategoriesQueryModel queryModel);
    Task UpdateAsync(UpdateIngredientCategoryCommandModel commandModel);
    Task AddAsync(AddIngredientCategoryCommandModel commandModel);
}
