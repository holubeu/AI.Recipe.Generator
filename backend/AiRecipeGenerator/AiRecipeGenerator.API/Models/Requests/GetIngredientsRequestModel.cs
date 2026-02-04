namespace AiRecipeGenerator.API.Models.Requests;

public class GetIngredientsRequestModel
{
    public string Name { get; set; }
    public int? CategoryId { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 25;
}
