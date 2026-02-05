using System.Text.Json.Serialization;

namespace AiRecipeGenerator.Application.Models.OpenRouter;

public class RecipeInfo
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("dishType")]
    public string DishType { get; set; }

    [JsonPropertyName("steps")]
    public string[] Steps { get; set; }

    [JsonPropertyName("cookingTime")]
    public CookingTime CookingTime { get; set; }
}
