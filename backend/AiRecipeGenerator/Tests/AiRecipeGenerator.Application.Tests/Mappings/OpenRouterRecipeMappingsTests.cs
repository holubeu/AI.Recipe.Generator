using AiRecipeGenerator.Application.Mappings;
using AiRecipeGenerator.Application.Models.OpenRouter;

using AutoFixture.Xunit2;

using Xunit;

namespace AiRecipeGenerator.Application.Tests.Mappings;

public class OpenRouterRecipeMappingsTests
{
    [Theory, AutoData]
    public void ToGeneratedRecipeDto_WithValidRecipeContent_ReturnsDto(RecipeContent recipeContent)
    {
        // Act
        var result = recipeContent.ToGeneratedRecipeDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(recipeContent.RecipeFound, result.RecipeFound);
        Assert.Equal(recipeContent.Message, result.Message);
        if (recipeContent.Recipe != null)
        {
            Assert.NotNull(result.Recipe);
            Assert.Equal(recipeContent.Recipe.Name, result.Recipe.Name);
            Assert.Equal(recipeContent.Recipe.Description, result.Recipe.Description);
        }
    }

    [Theory, AutoData]
    public void ToGeneratedRecipeDto_WithNullRecipe_ReturnsDto(string message)
    {
        // Arrange
        var recipeContent = new RecipeContent
        {
            RecipeFound = false,
            Message = message,
            Recipe = null
        };

        // Act
        var result = recipeContent.ToGeneratedRecipeDto();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.RecipeFound);
        Assert.Equal(message, result.Message);
        Assert.Null(result.Recipe);
    }

    [Fact]
    public void ToGeneratedRecipeDto_WithNullRecipeContent_ThrowsArgumentNullException()
    {
        // Arrange
        RecipeContent recipeContent = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => recipeContent.ToGeneratedRecipeDto());
    }

    [Theory, AutoData]
    public void ToGeneratedRecipeDto_WithNullCookingTime_ReturnsDtoWithNullCookingTime(string name, string description, string dishType, string[] steps)
    {
        // Arrange
        var recipeContent = new RecipeContent
        {
            RecipeFound = true,
            Message = "Recipe found",
            Recipe = new RecipeInfo
            {
                Name = name,
                Description = description,
                DishType = dishType,
                Steps = steps,
                CookingTime = null
            }
        };

        // Act
        var result = recipeContent.ToGeneratedRecipeDto();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Recipe);
        Assert.Null(result.Recipe.CookingTime);
    }
}
