using System.Net;
using System.Text.Json;

using AiRecipeGenerator.Application.Exceptions;
using AiRecipeGenerator.Application.Interfaces;
using AiRecipeGenerator.Application.Models.OpenRouter;
using AiRecipeGenerator.Application.Services;
using AiRecipeGenerator.Database.Models.Queries;

using NSubstitute;

using Xunit;

namespace AiRecipeGenerator.Application.Tests.Services;

public class OpenRouterServiceTests
{
    private const string TestApiKey = "test-api-key";

    [Fact]
    public async Task GenerateRecipeAsync_WithValidIngredients_ReturnsGeneratedRecipeDto()
    {
        // Arrange
        var mockApiKeyService = Substitute.For<IApiKeyService>();
        mockApiKeyService.GetLatestAsync().Returns(TestApiKey);

        var message = new OpenRouterMessage { Content = @"{ ""recipeFound"": true, ""message"": ""Recipe found"", ""recipe"": { ""name"": ""Pasta"", ""description"": ""Delicious pasta"", ""dishType"": ""Main"", ""steps"": [""Cook pasta"", ""Add sauce""], ""cookingTime"": { ""from"": 15, ""to"": 20 } } }" };
        var choice = new OpenRouterChoice { Message = message };
        var responseContent = new OpenRouterResponse { Choices = [choice] };

        var httpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(responseContent));
        var httpClient = new HttpClient(httpMessageHandler);

        var service = new OpenRouterService(mockApiKeyService, httpClient);
        var queryModel = new GenerateRecipeQueryModel { Ingredients = ["tomato", "pasta"] };

        // Act
        var result = await service.GenerateRecipeAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.RecipeFound);
        Assert.Equal("Recipe found", result.Message);
    }

    [Fact]
    public async Task GenerateRecipeAsync_WithNoRecipeFound_ReturnsDto()
    {
        // Arrange
        var mockApiKeyService = Substitute.For<IApiKeyService>();
        mockApiKeyService.GetLatestAsync().Returns(TestApiKey);

        var message = new OpenRouterMessage { Content = @"{ ""recipeFound"": false, ""message"": ""No recipe found"", ""recipe"": null }" };
        var choice = new OpenRouterChoice { Message = message };
        var responseContent = new OpenRouterResponse { Choices = [choice] };

        var httpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(responseContent));
        var httpClient = new HttpClient(httpMessageHandler);

        var service = new OpenRouterService(mockApiKeyService, httpClient);
        var queryModel = new GenerateRecipeQueryModel { Ingredients = ["unknown"] };

        // Act
        var result = await service.GenerateRecipeAsync(queryModel);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.RecipeFound);
        Assert.Null(result.Recipe);
    }

    [Fact]
    public async Task GenerateRecipeAsync_WithNullApiKey_ThrowsInvalidOperationException()
    {
        // Arrange
        var mockApiKeyService = Substitute.For<IApiKeyService>();
        mockApiKeyService.GetLatestAsync().Returns((string)null);

        var httpClient = new HttpClient();
        var service = new OpenRouterService(mockApiKeyService, httpClient);
        var queryModel = new GenerateRecipeQueryModel { Ingredients = ["test"] };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.GenerateRecipeAsync(queryModel));
    }

    [Fact]
    public async Task GenerateRecipeAsync_WithInvalidJsonResponse_ThrowsInvalidRecipeContentException()
    {
        // Arrange
        var mockApiKeyService = Substitute.For<IApiKeyService>();
        mockApiKeyService.GetLatestAsync().Returns(TestApiKey);

        var message = new OpenRouterMessage { Content = "{ invalid json }" };
        var choice = new OpenRouterChoice { Message = message };
        var responseContent = new OpenRouterResponse { Choices = [choice] };

        var httpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(responseContent));
        var httpClient = new HttpClient(httpMessageHandler);

        var service = new OpenRouterService(mockApiKeyService, httpClient);
        var queryModel = new GenerateRecipeQueryModel { Ingredients = ["test"] };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRecipeContentException>(() => service.GenerateRecipeAsync(queryModel));
    }

    [Fact]
    public async Task GenerateRecipeAsync_WithEmptyChoices_ThrowsInvalidRecipeContentException()
    {
        // Arrange
        var mockApiKeyService = Substitute.For<IApiKeyService>();
        mockApiKeyService.GetLatestAsync().Returns(TestApiKey);

        var responseContent = new OpenRouterResponse { Choices = [] };

        var httpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.OK, JsonSerializer.Serialize(responseContent));
        var httpClient = new HttpClient(httpMessageHandler);

        var service = new OpenRouterService(mockApiKeyService, httpClient);
        var queryModel = new GenerateRecipeQueryModel { Ingredients = ["test"] };

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRecipeContentException>(() => service.GenerateRecipeAsync(queryModel));
    }

    [Fact]
    public async Task GenerateRecipeAsync_WithHttpError_ThrowsHttpRequestException()
    {
        // Arrange
        var mockApiKeyService = Substitute.For<IApiKeyService>();
        mockApiKeyService.GetLatestAsync().Returns(TestApiKey);

        var httpMessageHandler = new MockHttpMessageHandler(HttpStatusCode.Unauthorized, "Unauthorized");
        var httpClient = new HttpClient(httpMessageHandler);

        var service = new OpenRouterService(mockApiKeyService, httpClient);
        var queryModel = new GenerateRecipeQueryModel { Ingredients = ["test"] };

        // Act & Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => service.GenerateRecipeAsync(queryModel));
    }
}

public class MockHttpMessageHandler(HttpStatusCode statusCode, string content) : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode = statusCode;
    private readonly string _content = content;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(_statusCode) { Content = new StringContent(_content) };
        return Task.FromResult(response);
    }
}
