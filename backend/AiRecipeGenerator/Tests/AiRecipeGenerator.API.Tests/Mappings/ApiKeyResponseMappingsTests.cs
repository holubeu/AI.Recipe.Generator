using AiRecipeGenerator.API.Mappings.Responses;

using AutoFixture.Xunit2;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Mappings;

public class ApiKeyResponseMappingsTests
{
    [Theory, AutoData]
    public void ToGetLatestApiKeyResponseModel_WithValidDto_ReturnsResponseModel(
        string apiKeyDto)
    {
        // Act
        var result = apiKeyDto.ToGetLatestApiKeyResponseModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(apiKeyDto, result.Key);
    }

    [Fact]
    public void ToGetLatestApiKeyResponseModel_WithNullDto_ThrowsArgumentNullException()
    {
        // Arrange
        string apiKeyDto = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => apiKeyDto.ToGetLatestApiKeyResponseModel());
    }

    [Theory, AutoData]
    public void ToGetLatestApiKeyResponseModel_WithEmptyString_ReturnsResponseModel(
        string apiKeyDto)
    {
        // Arrange - use AutoFixture but override with empty string
        var emptyKey = string.Empty;

        // Act
        var result = emptyKey.ToGetLatestApiKeyResponseModel();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(emptyKey, result.Key);
    }
}
