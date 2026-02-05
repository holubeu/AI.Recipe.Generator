namespace AiRecipeGenerator.API.Models.Responses;

public class RecipeContentResponseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string DishType { get; set; }
    public string[] Steps { get; set; }
    public CookingTimeResponseModel CookingTime { get; set; }
}
