namespace AiRecipeGenerator.API.Models.Responses;

public class GeneratedRecipeResponseModel
{
    public bool RecipeFound { get; set; }
    public string Message { get; set; }
    public RecipeContentResponseModel Recipe { get; set; }
}
