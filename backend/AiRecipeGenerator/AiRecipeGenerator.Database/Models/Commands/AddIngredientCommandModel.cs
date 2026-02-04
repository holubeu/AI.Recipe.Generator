namespace AiRecipeGenerator.Database.Models.Commands;

public class AddIngredientCommandModel
{
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public bool IsVisibleOnCard { get; set; }
    public string ImagePath { get; set; }
}
