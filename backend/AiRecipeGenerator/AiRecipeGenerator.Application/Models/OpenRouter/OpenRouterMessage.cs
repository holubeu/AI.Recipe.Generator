using System.Text.Json.Serialization;

namespace AiRecipeGenerator.Application.Models.OpenRouter;

public class OpenRouterMessage
{
    [JsonPropertyName("content")]
    public string Content { get; set; }
}
