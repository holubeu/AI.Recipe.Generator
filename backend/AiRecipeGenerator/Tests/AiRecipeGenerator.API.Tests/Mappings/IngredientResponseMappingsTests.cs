using AiRecipeGenerator.API.Mappings.Responses;
using AiRecipeGenerator.Application.Dtos;

using AutoFixture.Xunit2;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Mappings;

public class IngredientResponseMappingsTests
{
    [Theory, AutoData]
    public void ToPaginatedGetIngredientResponseModel_WithValidDto_ReturnsPaginatedResponseModel(
        PaginatedResultDto<IngredientDto> dto)
    {
        // Act
        var result = dto.ToPaginatedGetIngredientResponseModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Total, result.Total);
        Assert.NotNull(result.Items);
        Assert.Equal(dto.Items.Count(), result.Items.Count());
    }

    [Fact]
    public void ToPaginatedGetIngredientResponseModel_WithNullDto_ThrowsArgumentNullException()
    {
        // Arrange
        PaginatedResultDto<IngredientDto> dto = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dto.ToPaginatedGetIngredientResponseModel());
    }

    [Theory, AutoData]
    public void ToPaginatedGetIngredientResponseModel_WithValidItems_MapsAllProperties(
        PaginatedResultDto<IngredientDto> dto)
    {
        // Act
        var result = dto.ToPaginatedGetIngredientResponseModel();

        // Assert
        var resultItems = result.Items.ToList();
        var dtoItems = dto.Items.ToList();

        for (int i = 0; i < dtoItems.Count; i++)
        {
            Assert.Equal(dtoItems[i].Id, resultItems[i].Id);
            Assert.Equal(dtoItems[i].Name, resultItems[i].Name);
            Assert.Equal(dtoItems[i].CategoryId, resultItems[i].CategoryId);
            Assert.Equal(dtoItems[i].IsVisibleOnCard, resultItems[i].IsVisibleOnCard);
            Assert.Equal(dtoItems[i].ImagePath, resultItems[i].ImagePath);
        }
    }
}
