namespace AiRecipeGenerator.Database.Models.Commands;

public class UpdateIngredientCommandModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public bool IsVisibleOnCard { get; set; }
    public string ImagePath { get; set; }
}
