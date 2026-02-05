using AiRecipeGenerator.API.Mappings.Responses;
using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.Application.Dtos;

using AutoFixture.Xunit2;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Mappings;

public class GeneratedRecipeMappingsTests
{
    [Theory, AutoData]
    public void ToGenerateRecipeQueryModel_WithValidRequestModel_ReturnsQueryModel(GenerateRecipeRequestModel requestModel)
    {
        // Act
        var result = requestModel.ToGenerateRecipeQueryModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestModel.Ingredients.Length, result.Ingredients.Length);
        for (int i = 0; i < requestModel.Ingredients.Length; i++)
        {
            Assert.Equal(requestModel.Ingredients[i], result.Ingredients[i]);
        }
    }

    [Fact]
    public void ToGenerateRecipeQueryModel_WithNullRequestModel_ThrowsArgumentNullException()
    {
        // Arrange
        GenerateRecipeRequestModel requestModel = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => requestModel.ToGenerateRecipeQueryModel());
    }

    [Theory, AutoData]
    public void ToGeneratedRecipeResponseModel_WithValidDto_ReturnsResponseModel(GeneratedRecipeDto dto)
    {
        // Act
        var result = dto.ToGeneratedRecipeResponseModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.RecipeFound, result.RecipeFound);
        Assert.Equal(dto.Message, result.Message);
        if (dto.Recipe != null)
        {
            Assert.NotNull(result.Recipe);
            Assert.Equal(dto.Recipe.Name, result.Recipe.Name);
            Assert.Equal(dto.Recipe.Description, result.Recipe.Description);
            Assert.Equal(dto.Recipe.DishType, result.Recipe.DishType);
        }
    }

    [Theory, AutoData]
    public void ToGeneratedRecipeResponseModel_WithNullRecipe_ReturnsResponseModelWithNullRecipe(string message)
    {
        // Arrange
        var dto = new GeneratedRecipeDto
        {
            RecipeFound = false,
            Message = message,
            Recipe = null
        };

        // Act
        var result = dto.ToGeneratedRecipeResponseModel();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.RecipeFound);
        Assert.Equal(message, result.Message);
        Assert.Null(result.Recipe);
    }

    [Fact]
    public void ToGeneratedRecipeResponseModel_WithNullDto_ThrowsArgumentNullException()
    {
        // Arrange
        GeneratedRecipeDto dto = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dto.ToGeneratedRecipeResponseModel());
    }

    [Theory, AutoData]
    public void ToGeneratedRecipeResponseModel_WithNullCookingTime_ReturnsResponseModelWithNullCookingTime(
        string name, string description, string dishType, string[] steps)
    {
        // Arrange
        var dto = new GeneratedRecipeDto
        {
            RecipeFound = true,
            Message = "Recipe found",
            Recipe = new RecipeContentDto
            {
                Name = name,
                Description = description,
                DishType = dishType,
                Steps = steps,
                CookingTime = null
            }
        };

        // Act
        var result = dto.ToGeneratedRecipeResponseModel();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Recipe);
        Assert.Null(result.Recipe.CookingTime);
    }
}
