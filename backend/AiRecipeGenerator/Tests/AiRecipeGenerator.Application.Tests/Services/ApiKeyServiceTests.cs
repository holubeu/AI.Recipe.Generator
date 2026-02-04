using AiRecipeGenerator.Application.Services;
using AiRecipeGenerator.Database.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;

using AutoFixture.Xunit2;

using NSubstitute;

using Xunit;

namespace AiRecipeGenerator.Application.Tests.Services;

public class ApiKeyServiceTests
{
    [Theory, AutoData]
    public async Task GetLatestAsync_CallsRepositoryGetLatestAsync_ReturnsKey(string expectedKey)
    {
        // Arrange
        var mockRepository = Substitute.For<IApiKeyRepository>();
        mockRepository.GetLatestAsync().Returns(expectedKey);

        var service = new ApiKeyService(mockRepository);

        // Act
        var result = await service.GetLatestAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedKey, result);
        await mockRepository.Received(1).GetLatestAsync();
    }

    [Theory, AutoData]
    public async Task AddAsync_WithValidCommandModel_CallsRepositoryAddAsync(AddApiKeyCommandModel commandModel)
    {
        // Arrange
        var mockRepository = Substitute.For<IApiKeyRepository>();
        var service = new ApiKeyService(mockRepository);

        // Act
        await service.AddAsync(commandModel);

        // Assert
        await mockRepository.Received(1).AddAsync(commandModel);
    }
}
