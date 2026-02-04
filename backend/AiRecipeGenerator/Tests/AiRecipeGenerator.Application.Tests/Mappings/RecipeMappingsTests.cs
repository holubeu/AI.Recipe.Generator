using AiRecipeGenerator.Application.Mappings;
using AiRecipeGenerator.Database.Models;
using AiRecipeGenerator.Database.Models.Repository;

using Xunit;

namespace AiRecipeGenerator.Application.Tests.Mappings;

public class RecipeMappingsTests
{
    [Fact]
    public void ToRecipeDtosPaginated_WithValidPaginatedResult_ReturnsPaginatedRecipeDtos()
    {
        // Arrange
        var repositoryModels = new List<RecipeRepositoryModel>
        {
            new()
            {
                Id = 1,
                Name = "Pasta",
                Description = "Italian pasta",
                DishType = "Main",
                CookingTimeFrom = 20,
                CookingTimeTo = 30,
                Steps = "[\"Boil water\", \"Add pasta\", \"Drain\"]",
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            }
        };

        var paginatedResult = new PaginatedResultModel<RecipeRepositoryModel>
        {
            Items = repositoryModels,
            Total = 1
        };

        // Act
        var result = paginatedResult.ToRecipeDtosPaginated();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Items);
        Assert.Equal(1, result.Total);
        var recipeDto = result.Items.First();
        Assert.Equal("Pasta", recipeDto.Name);
        Assert.NotNull(recipeDto.Steps);
        Assert.Equal(3, recipeDto.Steps.Length);
        Assert.Equal("Boil water", recipeDto.Steps[0]);
    }

    [Fact]
    public void ToRecipeDtosPaginated_WithNullPaginatedResult_ThrowsArgumentNullException()
    {
        // Arrange
        PaginatedResultModel<RecipeRepositoryModel> paginatedResult = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => paginatedResult.ToRecipeDtosPaginated());
    }

    [Fact]
    public void ToRecipeDtosPaginated_WithInvalidJsonSteps_ThrowsInvalidOperationException()
    {
        // Arrange
        var repositoryModel = new RecipeRepositoryModel
        {
            Id = 1,
            Name = "Invalid Recipe",
            Description = "Recipe with invalid JSON",
            DishType = "Main",
            CookingTimeFrom = 20,
            CookingTimeTo = 30,
            Steps = "{invalid json}",
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        var paginatedResult = new PaginatedResultModel<RecipeRepositoryModel>
        {
            Items = new List<RecipeRepositoryModel> { repositoryModel },
            Total = 1
        };

        // Act & Assert
        var result = paginatedResult.ToRecipeDtosPaginated();
        var action = () => result.Items.ToList(); // Force enumeration to trigger the mapping

        var ex = Assert.Throws<InvalidOperationException>(action);
        Assert.Contains("Invalid JSON format in recipe steps", ex.Message);
    }

    [Fact]
    public void ToRecipeDtosPaginated_WithNullSteps_ThrowsInvalidOperationException()
    {
        // Arrange
        var repositoryModel = new RecipeRepositoryModel
        {
            Id = 1,
            Name = "Recipe",
            Description = "Test recipe",
            DishType = "Main",
            CookingTimeFrom = 20,
            CookingTimeTo = 30,
            Steps = null,
            CreatedOn = DateTime.UtcNow,
            UpdatedOn = DateTime.UtcNow
        };

        var paginatedResult = new PaginatedResultModel<RecipeRepositoryModel>
        {
            Items = new List<RecipeRepositoryModel> { repositoryModel },
            Total = 1
        };

        // Act & Assert
        var ex = Assert.Throws<InvalidOperationException>(() => paginatedResult.ToRecipeDtosPaginated().Items.ToList());
        Assert.Contains("Recipe steps JSON is null or empty", ex.Message);
    }
}
