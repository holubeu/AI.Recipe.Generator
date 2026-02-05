namespace AiRecipeGenerator.API.Models.Requests;

public class GetRecipesRequestModel
{
    public string Name { get; set; }
    public string DishType { get; set; }
    public int? MaxCookingTime { get; set; }
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 25;
}
