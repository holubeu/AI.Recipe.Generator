namespace AiRecipeGenerator.Database.Models.Commands;

public class AddRecipeCommandModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string DishType { get; set; }
    public int CookingTimeFrom { get; set; }
    public int CookingTimeTo { get; set; }
    public string Steps { get; set; }
}
