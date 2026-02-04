using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;

namespace AiRecipeGenerator.Application.Services;

public class ApiKeyService(IApiKeyRepository repository) : IApiKeyService
{
    public async Task<string> GetLatestAsync() => await repository.GetLatestAsync();

    public async Task AddAsync(AddApiKeyCommandModel commandModel) => await repository.AddAsync(commandModel);
}
