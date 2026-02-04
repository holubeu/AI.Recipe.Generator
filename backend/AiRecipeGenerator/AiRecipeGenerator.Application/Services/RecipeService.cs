using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Application.Mappings;
using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.Application.Services;

public class RecipeService(IRecipeRepository repository) : IRecipeService
{
    public async Task<PaginatedResultDto<RecipeDto>> GetAsync(GetRecipesQueryModel queryModel)
    {
        var result = await repository.GetAsync(queryModel);
        return result.ToRecipeDtosPaginated();
    }

    public async Task UpdateAsync(UpdateRecipeCommandModel commandModel) => await repository.UpdateAsync(commandModel);

    public async Task AddAsync(AddRecipeCommandModel commandModel) => await repository.AddAsync(commandModel);

    public async Task DeleteAsync(int id) => await repository.DeleteAsync(id);
}
