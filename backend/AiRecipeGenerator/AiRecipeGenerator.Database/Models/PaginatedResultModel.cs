namespace AiRecipeGenerator.Database.Models;

public class PaginatedResultModel<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Total { get; set; }
}
