namespace AiRecipeGenerator.API.Models.Responses;

public class IngredientByGroupResponseModel
{
    public string Name { get; set; }
    public bool IsVisibleOnCard { get; set; }
    public string ImagePath { get; set; }
}
