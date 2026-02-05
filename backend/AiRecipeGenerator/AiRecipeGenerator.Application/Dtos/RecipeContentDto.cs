namespace AiRecipeGenerator.Application.Dtos;

public class RecipeContentDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string DishType { get; set; }
    public string[] Steps { get; set; }
    public CookingTimeDto CookingTime { get; set; }
}
