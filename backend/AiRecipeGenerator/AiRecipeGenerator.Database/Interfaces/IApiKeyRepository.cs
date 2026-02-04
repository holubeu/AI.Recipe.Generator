using AiRecipeGenerator.Database.Models.Commands;

namespace AiRecipeGenerator.Database.Interfaces;

public interface IApiKeyRepository
{
    Task<string> GetLatestAsync();
    Task AddAsync(AddApiKeyCommandModel commandModel);
}
