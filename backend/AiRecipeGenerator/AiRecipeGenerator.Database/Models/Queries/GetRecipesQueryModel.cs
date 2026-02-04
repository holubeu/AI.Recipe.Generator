namespace AiRecipeGenerator.Database.Models.Queries;

public class GetRecipesQueryModel : PaginationModel
{
    public string Name { get; set; }
    public string DishType { get; set; }
    public int? CookingTimeFrom { get; set; }
    public int? CookingTimeTo { get; set; }
}
