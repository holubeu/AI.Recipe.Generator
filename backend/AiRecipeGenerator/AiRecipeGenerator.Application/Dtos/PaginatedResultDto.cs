namespace AiRecipeGenerator.Application.Dtos;

public class PaginatedResultDto<T>
{
    public IEnumerable<T> Items { get; set; }
    public int Total { get; set; }
}
