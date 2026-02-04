using AiRecipeGenerator.Application.Mappings;
using AiRecipeGenerator.Database.Models.Repository;

using Xunit;

namespace AiRecipeGenerator.Application.Tests.Mappings;

public class ApiKeyMappingsTests
{
    [Fact]
    public void ToApiKeyDto_WithValidRepositoryModel_ReturnsApiKeyDto()
    {
        // Arrange
        var repositoryModel = new ApiKeyRepositoryModel
        {
            Id = 1,
            Key = "test-key-12345",
            CreatedOn = DateTime.UtcNow
        };

        // Act
        var result = repositoryModel.ToApiKeyDto();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("test-key-12345", result.Key);
        Assert.Equal(repositoryModel.CreatedOn, result.CreatedOn);
    }

    [Fact]
    public void ToApiKeyDto_WithNullRepositoryModel_ThrowsArgumentNullException()
    {
        // Arrange
        ApiKeyRepositoryModel repositoryModel = null;

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => repositoryModel.ToApiKeyDto());
    }
}
