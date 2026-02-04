using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Application.Mappings;
using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.Application.Services;

public class IngredientService(IIngredientRepository repository) : IIngredientService
{
    public async Task<PaginatedResultDto<IngredientDto>> GetAsync(GetIngredientsQueryModel queryModel)
    {
        var result = await repository.GetAsync(queryModel);
        return result.ToIngredientDtosPaginated();
    }

    public async Task UpdateAsync(UpdateIngredientCommandModel commandModel) => await repository.UpdateAsync(commandModel);

    public async Task AddAsync(AddIngredientCommandModel commandModel) => await repository.AddAsync(commandModel);

    public async Task DeleteAsync(int id) => await repository.DeleteAsync(id);

    public async Task<IEnumerable<GetAllIngredientsDto>> GetAllAsync()
    {
        var result = await repository.GetAllAsync();
        var groupedByCategory = result.GroupBy(x => x.CategoryName);
        return groupedByCategory.ToGetAllIngredientsDtos();
    }
}
