using AiRecipeGenerator.API.Mappings.Requests;
using AiRecipeGenerator.API.Models.Requests;

using AutoFixture.Xunit2;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Mappings;

public class RecipeRequestMappingsTests
{
    [Theory, AutoData]
    public void ToGetRecipesQueryModel_WithValidRequestModel_ReturnsQueryModel(
        GetRecipesRequestModel requestModel)
    {
        // Act
        var result = requestModel.ToGetRecipesQueryModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestModel.Name, result.Name);
        Assert.Equal(requestModel.DishType, result.DishType);
        Assert.Equal(requestModel.MaxCookingTime, result.MaxCookingTime);
        Assert.Equal(requestModel.Skip, result.Skip);
        Assert.Equal(requestModel.Take, result.Take);
    }

    [Fact]
    public void ToGetRecipesQueryModel_WithNullRequestModel_ThrowsArgumentNullException()
    {
        // Arrange
        GetRecipesRequestModel requestModel = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => requestModel.ToGetRecipesQueryModel());
    }

    [Theory, AutoData]
    public void ToAddRecipeCommandModel_WithValidRequestModel_ReturnsCommandModel(
        AddRecipeRequestModel requestModel)
    {
        // Act
        var result = requestModel.ToAddRecipeCommandModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestModel.Name, result.Name);
        Assert.Equal(requestModel.Description, result.Description);
        Assert.Equal(requestModel.DishType, result.DishType);
        Assert.Equal(requestModel.CookingTimeFrom, result.CookingTimeFrom);
        Assert.Equal(requestModel.CookingTimeTo, result.CookingTimeTo);
    }

    [Fact]
    public void ToAddRecipeCommandModel_WithNullRequestModel_ThrowsArgumentNullException()
    {
        // Arrange
        AddRecipeRequestModel requestModel = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => requestModel.ToAddRecipeCommandModel());
    }

    [Theory, AutoData]
    public void ToUpdateRecipeCommandModel_WithValidRequestModel_ReturnsCommandModel(
        UpdateRecipeRequestModel requestModel)
    {
        // Act
        var result = requestModel.ToUpdateRecipeCommandModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(requestModel.Id, result.Id);
        Assert.Equal(requestModel.Name, result.Name);
        Assert.Equal(requestModel.Description, result.Description);
        Assert.Equal(requestModel.DishType, result.DishType);
        Assert.Equal(requestModel.CookingTimeFrom, result.CookingTimeFrom);
        Assert.Equal(requestModel.CookingTimeTo, result.CookingTimeTo);
    }

    [Fact]
    public void ToUpdateRecipeCommandModel_WithNullRequestModel_ThrowsArgumentNullException()
    {
        // Arrange
        UpdateRecipeRequestModel requestModel = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => requestModel.ToUpdateRecipeCommandModel());
    }
}
