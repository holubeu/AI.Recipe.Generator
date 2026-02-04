using AiRecipeGenerator.Application.Mappings;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Repository;

using Xunit;

namespace AiRecipeGenerator.Application.Tests.Mappings;

public class IngredientMappingsTests
{
    [Fact]
    public void ToIngredientDtosPaginated_WithValidPaginatedResult_ReturnsPaginatedIngredientDtos()
    {
        // Arrange
        var repositoryModels = new List<IngredientRepositoryModel>
        {
            new()
            {
                Id = 1,
                Name = "Tomato",
                CategoryId = 1,
                IsVisibleOnCard = true,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow,
                ImagePath = "/images/tomato.jpg"
            }
        };

        var paginatedResult = new PaginatedResultModel<IngredientRepositoryModel>
        {
            Items = repositoryModels,
            Total = 1
        };

        // Act
        var result = paginatedResult.ToIngredientDtosPaginated();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.Total);
        var ingredientDto = result.Items.First();
        Assert.Equal("Tomato", ingredientDto.Name);
        Assert.Equal(1, ingredientDto.CategoryId);
        Assert.True(ingredientDto.IsVisibleOnCard);
        Assert.Equal("/images/tomato.jpg", ingredientDto.ImagePath);
    }

    [Fact]
    public void ToIngredientDtosPaginated_WithNullPaginatedResult_ThrowsArgumentNullException()
    {
        // Arrange
        PaginatedResultModel<IngredientRepositoryModel> paginatedResult = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => paginatedResult.ToIngredientDtosPaginated());
    }
}
