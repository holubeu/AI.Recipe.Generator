namespace AiRecipeGenerator.API.Models.Requests;

public class GetIngredientCategoriesRequestModel
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 25;
}
