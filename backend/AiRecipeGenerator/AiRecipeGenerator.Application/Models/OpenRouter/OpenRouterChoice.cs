using System.Text.Json.Serialization;

namespace AiRecipeGenerator.Application.Models.OpenRouter;

public class OpenRouterChoice
{
    [JsonPropertyName("message")]
    public OpenRouterMessage Message { get; set; }
}
