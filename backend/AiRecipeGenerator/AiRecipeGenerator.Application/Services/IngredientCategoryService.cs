using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Application.Mappings;
using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.Application.Services;

public class IngredientCategoryService(IIngredientCategoryRepository repository) : IIngredientCategoryService
{
    public async Task<PaginatedResultDto<IngredientCategoryDto>> GetAllAsync(GetIngredientCategoriesQueryModel queryModel)
    {
        var result = await repository.GetAllAsync(queryModel);
        return result.ToIngredientCategoryDtosPaginated();
    }

    public async Task UpdateAsync(UpdateIngredientCategoryCommandModel commandModel) => await repository.UpdateAsync(commandModel);

    public async Task AddAsync(AddIngredientCategoryCommandModel commandModel) => await repository.AddAsync(commandModel);
}
