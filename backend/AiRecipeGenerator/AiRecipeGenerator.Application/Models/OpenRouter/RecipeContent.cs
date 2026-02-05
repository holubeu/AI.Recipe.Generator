using System.Text.Json.Serialization;

namespace AiRecipeGenerator.Application.Models.OpenRouter;

public class RecipeContent
{
    [JsonPropertyName("recipeFound")]
    public bool RecipeFound { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("recipe")]
    public RecipeInfo Recipe { get; set; }
}
