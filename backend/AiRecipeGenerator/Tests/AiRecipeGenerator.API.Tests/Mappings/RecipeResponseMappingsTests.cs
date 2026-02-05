using AiRecipeGenerator.API.Mappings.Responses;
using AiRecipeGenerator.Application.Dtos;

using AutoFixture.Xunit2;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Mappings;

public class RecipeResponseMappingsTests
{
    [Theory, AutoData]
    public void ToPaginatedGetRecipeResponseModel_WithValidDto_ReturnsPaginatedResponseModel(
        PaginatedResultDto<RecipeDto> dto)
    {
        // Act
        var result = dto.ToPaginatedGetRecipeResponseModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(dto.Total, result.Total);
        Assert.NotNull(result.Items);
        Assert.Equal(dto.Items.Count(), result.Items.Count());
    }

    [Fact]
    public void ToPaginatedGetRecipeResponseModel_WithNullDto_ThrowsArgumentNullException()
    {
        // Arrange
        PaginatedResultDto<RecipeDto> dto = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => dto.ToPaginatedGetRecipeResponseModel());
    }

    [Theory, AutoData]
    public void ToPaginatedGetRecipeResponseModel_WithValidItems_MapsAllProperties(
        PaginatedResultDto<RecipeDto> dto)
    {
        // Act
        var result = dto.ToPaginatedGetRecipeResponseModel();

        // Assert
        var resultItems = result.Items.ToList();
        var dtoItems = dto.Items.ToList();

        for (int i = 0; i < dtoItems.Count; i++)
        {
            Assert.Equal(dtoItems[i].Id, resultItems[i].Id);
            Assert.Equal(dtoItems[i].Name, resultItems[i].Name);
            Assert.Equal(dtoItems[i].Description, resultItems[i].Description);
            Assert.Equal(dtoItems[i].DishType, resultItems[i].DishType);
            Assert.Equal(dtoItems[i].CookingTimeFrom, resultItems[i].CookingTimeFrom);
            Assert.Equal(dtoItems[i].CookingTimeTo, resultItems[i].CookingTimeTo);
        }
    }
}
