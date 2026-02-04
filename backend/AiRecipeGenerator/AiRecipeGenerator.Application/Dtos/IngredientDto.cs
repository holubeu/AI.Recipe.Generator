namespace AiRecipeGenerator.Application.Dtos;

public class IngredientDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public bool IsVisibleOnCard { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
    public string ImagePath { get; set; }
}
