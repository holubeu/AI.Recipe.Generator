using System.Text.Json.Serialization;

namespace AiRecipeGenerator.Application.Models.OpenRouter;

public class CookingTime
{
    [JsonPropertyName("from")]
    public int From { get; set; }

    [JsonPropertyName("to")]
    public int To { get; set; }
}
