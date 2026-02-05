using AiRecipeGenerator.API.Mappings.Responses;
using AiRecipeGenerator.Application.Dtos;

using AutoFixture.Xunit2;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Mappings;

public class IngredientCategoryResponseMappingsTests
{
    [Theory, AutoData]
    public void ToPaginatedGetIngredientCategoryResponseModel_WithValidDto_ReturnsPaginatedResponseModel(
        PaginatedResultDto<IngredientCategoryDto> dto)
    {
        // Act
        var result = dto.ToPaginatedGetIngredientCategoryResponseModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Total, result.Total);
        Assert.NotNull(result.Items);
        Assert.Equal(dto.Items.Count(), result.Items.Count());
    }

    [Fact]
    public void ToPaginatedGetIngredientCategoryResponseModel_WithNullDto_ThrowsArgumentNullException()
    {
        // Arrange
        PaginatedResultDto<IngredientCategoryDto> dto = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dto.ToPaginatedGetIngredientCategoryResponseModel());
    }

    [Theory, AutoData]
    public void ToPaginatedGetIngredientCategoryResponseModel_WithValidItems_MapsAllProperties(
        PaginatedResultDto<IngredientCategoryDto> dto)
    {
        // Act
        var result = dto.ToPaginatedGetIngredientCategoryResponseModel();

        // Assert
        var resultItems = result.Items.ToList();
        var dtoItems = dto.Items.ToList();

        for (int i = 0; i < dtoItems.Count; i++)
        {
            Assert.Equal(dtoItems[i].Id, resultItems[i].Id);
            Assert.Equal(dtoItems[i].Name, resultItems[i].Name);
        }
    }
}
