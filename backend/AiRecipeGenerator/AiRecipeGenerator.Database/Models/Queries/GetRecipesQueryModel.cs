namespace AiRecipeGenerator.Database.Models.Queries;

public class GetRecipesQueryModel : PaginationModel
{
    public string Name { get; set; }
    public string DishType { get; set; }
    public int? MaxCookingTime { get; set; }
}
