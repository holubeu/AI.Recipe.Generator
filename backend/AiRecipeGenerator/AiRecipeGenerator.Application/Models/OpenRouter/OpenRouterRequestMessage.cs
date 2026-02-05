using System.Text.Json.Serialization;

namespace AiRecipeGenerator.Application.Models.OpenRouter;

public class OpenRouterRequestMessage
{
    [JsonPropertyName("role")]
    public string Role { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}
