namespace AiRecipeGenerator.Database.Models.Commands;

public class UpdateRecipeCommandModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string DishType { get; set; }
    public int CookingTimeFrom { get; set; }
    public int CookingTimeTo { get; set; }
    public string Steps { get; set; }
}
