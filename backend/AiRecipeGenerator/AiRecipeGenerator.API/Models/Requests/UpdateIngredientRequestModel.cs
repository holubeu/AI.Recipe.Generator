namespace AiRecipeGenerator.API.Models.Requests;

public class UpdateIngredientRequestModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public bool IsVisibleOnCard { get; set; }
    public string ImagePath { get; set; }
}
