namespace AiRecipeGenerator.Application.Dtos;

public class GeneratedRecipeDto
{
    public bool RecipeFound { get; set; }
    public string Message { get; set; }
    public RecipeContentDto Recipe { get; set; }
}
