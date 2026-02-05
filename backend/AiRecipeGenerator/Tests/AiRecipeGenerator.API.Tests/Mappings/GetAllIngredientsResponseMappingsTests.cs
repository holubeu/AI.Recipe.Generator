using AiRecipeGenerator.API.Mappings.Responses;
using AiRecipeGenerator.Application.Dtos;

using AutoFixture.Xunit2;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Mappings;

public class GetAllIngredientsResponseMappingsTests
{
    [Theory, AutoData]
    public void ToGetAllIngredientsResponseModels_WithValidDtos_ReturnsResponseModels(
        List<GetAllIngredientsDto> dtos)
    {
        // Act
        var result = dtos.ToGetAllIngredientsResponseModels();

        // Assert
        Assert.NotNull(result);
        var resultList = result.ToList();
        Assert.Equal(dtos.Count, resultList.Count);
    }

    [Fact]
    public void ToGetAllIngredientsResponseModels_WithNullDtos_ThrowsArgumentNullException()
    {
        // Arrange
        List<GetAllIngredientsDto> dtos = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dtos.ToGetAllIngredientsResponseModels());
    }

    [Theory, AutoData]
    public void ToGetAllIngredientsResponseModels_WithValidDtos_MapsAllProperties(
        GetAllIngredientsDto dto)
    {
        // Arrange
        var dtos = new[] { dto };

        // Act
        var result = dtos.ToGetAllIngredientsResponseModels();

        // Assert
        var resultList = result.ToList();
        Assert.Single(resultList);
        
        var resultItem = resultList[0];
        Assert.Equal(dto.Category, resultItem.Category);
        Assert.NotNull(resultItem.Ingredients);
        Assert.Equal(dto.Ingredients.Count(), resultItem.Ingredients.Count());

        var resultIngredients = resultItem.Ingredients.ToList();
        var dtoIngredients = dto.Ingredients.ToList();

        for (int i = 0; i < dtoIngredients.Count; i++)
        {
            Assert.Equal(dtoIngredients[i].Name, resultIngredients[i].Name);
            Assert.Equal(dtoIngredients[i].IsVisibleOnCard, resultIngredients[i].IsVisibleOnCard);
            Assert.Equal(dtoIngredients[i].ImagePath, resultIngredients[i].ImagePath);
        }
    }

    [Theory, AutoData]
    public void ToGetAllIngredientsResponseModels_WithEmptyDtos_ReturnsEmptyCollection()
    {
        // Arrange
        var dtos = new List<GetAllIngredientsDto>();

        // Act
        var result = dtos.ToGetAllIngredientsResponseModels();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
