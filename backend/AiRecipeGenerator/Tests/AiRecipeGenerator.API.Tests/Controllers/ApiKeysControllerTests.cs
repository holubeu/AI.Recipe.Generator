using AiRecipeGenerator.API.Controllers;
using AiRecipeGenerator.API.Models.Requests;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Database.Models.Commands;

using AutoFixture.Xunit2;

using Microsoft.AspNetCore.Mvc;

using NSubstitute;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Controllers;

public class ApiKeysControllerTests
{
    [Theory, AutoData]
    public async Task GetLatestAsync_CallsService_ReturnsOkWithApiKey(string apiKey)
    {
        // Arrange
        var mockService = Substitute.For<IApiKeyService>();
        mockService.GetLatestAsync().Returns(apiKey);

        var controller = new ApiKeysController(mockService);

        // Act
        var result = await controller.GetLatestAsync();

        // Assert
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        await mockService.Received(1).GetLatestAsync();
    }

    [Theory, AutoData]
    public async Task AddAsync_WithValidRequestModel_CallsServiceAndReturnsNoContent(AddApiKeyRequestModel requestModel)
    {
        // Arrange
        var mockService = Substitute.For<IApiKeyService>();
        var controller = new ApiKeysController(mockService);

        // Act
        var result = await controller.AddAsync(requestModel);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await mockService.Received(1).AddAsync(Arg.Any<AddApiKeyCommandModel>());
    }
}
