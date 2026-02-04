namespace AiRecipeGenerator.Application.Dtos;

public class RecipeDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string DishType { get; set; }
    public int CookingTimeFrom { get; set; }
    public int CookingTimeTo { get; set; }
    public string[] Steps { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}
