using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models.Commands;

namespace AiRecipeGenerator.Application.Interfaces;

public interface IApiKeyService
{
    Task<string> GetLatestAsync();
    Task AddAsync(AddApiKeyCommandModel commandModel);
}
