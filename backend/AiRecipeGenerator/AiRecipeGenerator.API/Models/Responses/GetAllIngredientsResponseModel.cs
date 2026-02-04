namespace AiRecipeGenerator.API.Models.Responses;

public class GetAllIngredientsResponseModel
{
    public string Category { get; set; }
    public IEnumerable<IngredientByGroupResponseModel> Ingredients { get; set; }
}
