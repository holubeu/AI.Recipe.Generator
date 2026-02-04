namespace AiRecipeGenerator.API.Models.Responses;

public class GetIngredientCategoryResponseModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}
