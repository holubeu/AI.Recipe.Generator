namespace AiRecipeGenerator.API.Models.Responses;

public class PaginatedResponseModel<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Total { get; set; }
}
