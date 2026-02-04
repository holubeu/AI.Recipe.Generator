using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.Application.Interfaces;

public interface IIngredientCategoryService
{
    Task<PaginatedResultDto<IngredientCategoryDto>> GetAllAsync(GetIngredientCategoriesQueryModel queryModel);
    Task UpdateAsync(UpdateIngredientCategoryCommandModel commandModel);
    Task AddAsync(AddIngredientCategoryCommandModel commandModel);
}
