using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models.Commands;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.Application.Interfaces;

public interface IIngredientService
{
    Task<PaginatedResultDto<IngredientDto>> GetAsync(GetIngredientsQueryModel queryModel);
    Task UpdateAsync(UpdateIngredientCommandModel commandModel);
    Task AddAsync(AddIngredientCommandModel commandModel);
    Task DeleteAsync(int id);
    Task<IEnumerable<GetAllIngredientsDto>> GetAllAsync();
}
