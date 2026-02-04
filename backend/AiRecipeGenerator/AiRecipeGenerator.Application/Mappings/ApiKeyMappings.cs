using AiRecipeGenerator.Application.Dtos;
using AiRecipeGenerator.Database.Models.Repository;

namespace AiRecipeGenerator.Application.Mappings;

public static class ApiKeyMappings
{
    public static ApiKeyDto ToApiKeyDto(this ApiKeyRepositoryModel repositoryModel)
    {
        ArgumentNullException.ThrowIfNull(repositoryModel);

        return new()
        {
            Id = repositoryModel.Id,
            Key = repositoryModel.Key,
            CreatedOn = repositoryModel.CreatedOn
        };
    }
}
