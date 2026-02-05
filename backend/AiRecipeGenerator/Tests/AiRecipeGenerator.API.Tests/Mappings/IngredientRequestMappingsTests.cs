using AiRecipeGenerator.API.Mappings.Requests;
using AiRecipeGenerator.API.Models.Requests;

using AutoFixture.Xunit2;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Mappings;

public class IngredientRequestMappingsTests
{
    [Theory, AutoData]
    public void ToGetIngredientsQueryModel_WithValidRequestModel_ReturnsQueryModel(
        GetIngredientsRequestModel requestModel)
    {
        // Act
        var result = requestModel.ToGetIngredientsQueryModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestModel.Name, result.Name);
        Assert.Equal(requestModel.CategoryId, result.CategoryId);
        Assert.Equal(requestModel.Skip, result.Skip);
        Assert.Equal(requestModel.Take, result.Take);
    }

    [Fact]
    public void ToGetIngredientsQueryModel_WithNullRequestModel_ThrowsArgumentNullException()
    {
        // Arrange
        GetIngredientsRequestModel requestModel = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => requestModel.ToGetIngredientsQueryModel());
    }

    [Theory, AutoData]
    public void ToAddIngredientCommandModel_WithValidRequestModel_ReturnsCommandModel(
        AddIngredientRequestModel requestModel)
    {
        // Act
        var result = requestModel.ToAddIngredientCommandModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestModel.Name, result.Name);
        Assert.Equal(requestModel.CategoryId, result.CategoryId);
        Assert.Equal(requestModel.IsVisibleOnCard, result.IsVisibleOnCard);
        Assert.Equal(requestModel.ImagePath, result.ImagePath);
    }

    [Fact]
    public void ToAddIngredientCommandModel_WithNullRequestModel_ThrowsArgumentNullException()
    {
        // Arrange
        AddIngredientRequestModel requestModel = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => requestModel.ToAddIngredientCommandModel());
    }

    [Theory, AutoData]
    public void ToUpdateIngredientCommandModel_WithValidRequestModel_ReturnsCommandModel(
        UpdateIngredientRequestModel requestModel)
    {
        // Act
        var result = requestModel.ToUpdateIngredientCommandModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestModel.Id, result.Id);
        Assert.Equal(requestModel.Name, result.Name);
        Assert.Equal(requestModel.CategoryId, result.CategoryId);
        Assert.Equal(requestModel.IsVisibleOnCard, result.IsVisibleOnCard);
        Assert.Equal(requestModel.ImagePath, result.ImagePath);
    }

    [Fact]
    public void ToUpdateIngredientCommandModel_WithNullRequestModel_ThrowsArgumentNullException()
    {
        // Arrange
        UpdateIngredientRequestModel requestModel = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => requestModel.ToUpdateIngredientCommandModel());
    }
}
