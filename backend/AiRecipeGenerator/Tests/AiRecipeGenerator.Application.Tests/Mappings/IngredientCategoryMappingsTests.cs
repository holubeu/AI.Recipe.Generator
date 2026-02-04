using AiRecipeGenerator.Application.Mappings;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Repository;

using Xunit;

namespace AiRecipeGenerator.Application.Tests.Mappings;

public class IngredientCategoryMappingsTests
{
    [Fact]
    public void ToIngredientCategoryDtosPaginated_WithValidPaginatedResult_ReturnsPaginatedIngredientCategoryDtos()
    {
        // Arrange
        var repositoryModels = new List<IngredientCategoryRepositoryModel>
        {
            new()
            {
                Id = 1,
                Name = "Vegetables",
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            }
        };

        var paginatedResult = new PaginatedResultModel<IngredientCategoryRepositoryModel>
        {
            Items = repositoryModels,
            Total = 1
        };

        // Act
        var result = paginatedResult.ToIngredientCategoryDtosPaginated();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.Total);
        var categoryDto = result.Items.First();
        Assert.Equal("Vegetables", categoryDto.Name);
        Assert.Equal(1, categoryDto.Id);
    }

    [Fact]
    public void ToIngredientCategoryDtosPaginated_WithNullPaginatedResult_ThrowsArgumentNullException()
    {
        // Arrange
        PaginatedResultModel<IngredientCategoryRepositoryModel> paginatedResult = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => paginatedResult.ToIngredientCategoryDtosPaginated());
    }
}
