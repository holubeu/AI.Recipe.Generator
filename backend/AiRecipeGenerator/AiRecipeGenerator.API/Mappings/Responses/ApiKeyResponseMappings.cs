using AiRecipeGenerator.API.Models.Responses;

namespace AiRecipeGenerator.API.Mappings.Responses;

public static class ApiKeyResponseMappings
{
    public static GetLatestApiKeyResponseModel ToGetLatestApiKeyResponseModel(this string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        return new()
        {
            Key = key
        };
    }
}
