using System.Text.Json.Serialization;

namespace AiRecipeGenerator.Application.Models.OpenRouter;

public class OpenRouterRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; }

    [JsonPropertyName("messages")]
    public OpenRouterRequestMessage[] Messages { get; set; }
}
