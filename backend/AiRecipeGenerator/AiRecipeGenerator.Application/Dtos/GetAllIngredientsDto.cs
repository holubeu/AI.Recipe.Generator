namespace AiRecipeGenerator.Application.Dtos;

public class GetAllIngredientsDto
{
    public string Category { get; set; }
    public IEnumerable<IngredientByGroupDto> Ingredients { get; set; }
}
