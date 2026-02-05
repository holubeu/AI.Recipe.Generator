using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models.Queries;

namespace AiRecipeGenerator.Application.Interfaces;

public interface IOpenRouterService
{
    Task<GeneratedRecipeDto> GenerateRecipeAsync(GenerateRecipeQueryModel queryModel);
}
