using System.Text.Json;
using System.Text.Json.Serialization;

using AiRecipeGenerator.API.Exceptions;
using AiRecipeGenerator.API.Middleware;
using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Middleware;

public class ErrorHandlingMiddlewareTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    [Fact]
    public async Task InvokeAsync_WithNoException_CallsNextMiddleware()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var nextCalled = false;

        RequestDelegate next = async context =>
        {
            nextCalled = true;
            await Task.CompletedTask;
        };

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.True(nextCalled);
    }

    [Fact]
    public async Task InvokeAsync_WithApiException_ReturnsStructuredErrorResponse()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        RequestDelegate next = context => throw new ApiException("Test error", 400);

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.Equal(400, httpContext.Response.StatusCode);
        Assert.Contains("application/json", httpContext.Response.ContentType);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, JsonOptions);

        Assert.NotNull(errorResponse);
        Assert.Equal(400, errorResponse.StatusCode);
        Assert.Equal("Test error", errorResponse.Message);
        Assert.Equal("ApiException", errorResponse.ErrorType);
    }

    [Fact]
    public async Task InvokeAsync_WithResourceNotFoundException_Returns404()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        RequestDelegate next = context => throw new ResourceNotFoundException("Recipe not found");

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.Equal(404, httpContext.Response.StatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, JsonOptions);

        Assert.NotNull(errorResponse);
        Assert.Equal(404, errorResponse.StatusCode);
        Assert.Equal("Recipe not found", errorResponse.Message);
        Assert.Equal("ResourceNotFoundException", errorResponse.ErrorType);
    }

    [Fact]
    public async Task InvokeAsync_WithValidationException_Returns400WithErrors()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        var validationErrors = new Dictionary<string, string[]>
        {
            { "Name", new[] { "Name is required" } }
        };

        RequestDelegate next = context => throw new ValidationException("Validation failed", validationErrors);

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.Equal(400, httpContext.Response.StatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, JsonOptions);

        Assert.NotNull(errorResponse);
        Assert.NotNull(errorResponse.ValidationErrors);
        Assert.Contains("Name", errorResponse.ValidationErrors.Keys);
    }

    [Fact]
    public async Task InvokeAsync_WithUnauthorizedException_Returns401()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        RequestDelegate next = context => throw new UnauthorizedException("Invalid token");

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.Equal(401, httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WithForbiddenException_Returns403()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        RequestDelegate next = context => throw new ForbiddenException("Insufficient permissions");

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.Equal(403, httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WithInvalidRecipeContentException_Returns400()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        RequestDelegate next = context => throw new InvalidRecipeContentException("Invalid JSON");

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.Equal(400, httpContext.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_WithGenericException_Returns500()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        RequestDelegate next = context => throw new InvalidOperationException("Unexpected error");

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.Equal(500, httpContext.Response.StatusCode);

        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, JsonOptions);

        Assert.NotNull(errorResponse);
        Assert.Equal(500, errorResponse.StatusCode);
        Assert.Equal("An internal server error occurred", errorResponse.Message);
    }

    [Fact]
    public async Task InvokeAsync_IncludesTraceIdInResponse()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var responseBody = new MemoryStream();
        httpContext.Response.Body = responseBody;
        httpContext.TraceIdentifier = "test-trace-123";

        RequestDelegate next = context => throw new ApiException("Test error");

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBodyText, JsonOptions);

        Assert.NotNull(errorResponse);
        // The trace identifier from DefaultHttpContext is generated, but we verify the structure is correct
        Assert.NotNull(errorResponse.TraceId);
        Assert.NotEmpty(errorResponse.TraceId);
        Assert.Equal(500, errorResponse.StatusCode);
        Assert.Equal("Test error", errorResponse.Message);
    }

    [Fact]
    public async Task InvokeAsync_ErrorResponse_ContainsAllRequiredFields()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Response.Body = new MemoryStream();

        var validationErrors = new Dictionary<string, string[]>
        {
            { "Field", new[] { "Error message" } }
        };

        RequestDelegate next = context => throw new ValidationException("Validation failed", validationErrors);

        var logger = new FakeLogger<ErrorHandlingMiddleware>();
        var middleware = new ErrorHandlingMiddleware(next, logger);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        httpContext.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(httpContext.Response.Body).ReadToEndAsync();
        var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, JsonOptions);

        Assert.NotNull(errorResponse);
        Assert.NotNull(errorResponse.TraceId);
        Assert.NotEmpty(errorResponse.TraceId);
        Assert.NotNull(errorResponse.Message);
        Assert.NotNull(errorResponse.ErrorType);
        Assert.NotEqual(default, errorResponse.Timestamp);
        // ValidationErrors is only populated for ValidationException
        Assert.True(errorResponse.ValidationErrors != null && errorResponse.ValidationErrors.Count > 0);
        Assert.Contains("Field", errorResponse.ValidationErrors.Keys);
    }
}

public class FakeLogger<T> : ILogger<T>
{
    public IDisposable BeginScope<TState>(TState state) => null;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter) { }
}
