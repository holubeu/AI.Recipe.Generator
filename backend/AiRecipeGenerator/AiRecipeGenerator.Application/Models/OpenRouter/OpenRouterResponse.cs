using System.Text.Json.Serialization;

namespace AiRecipeGenerator.Application.Models.OpenRouter;

public class OpenRouterResponse
{
    [JsonPropertyName("choices")]
    public OpenRouterChoice[] Choices { get; set; }
}
