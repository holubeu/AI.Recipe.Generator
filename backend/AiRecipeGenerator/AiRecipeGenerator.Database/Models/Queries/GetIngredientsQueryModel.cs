namespace AiRecipeGenerator.Database.Models.Queries;

public class GetIngredientsQueryModel : PaginationModel
{
    public string Name { get; set; }
    public int? CategoryId { get; set; }
}
