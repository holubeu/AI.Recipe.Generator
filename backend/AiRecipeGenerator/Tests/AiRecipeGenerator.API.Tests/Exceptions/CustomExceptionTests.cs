using AiRecipeGenerator.API.Exceptions;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Exceptions;

public class CustomExceptionTests
{
    [Fact]
    public void ApiException_WithMessage_SetsMessageAndDefaultStatusCode()
    {
        // Arrange & Act
        var exception = new ApiException("Test error");

        // Assert
        Assert.Equal("Test error", exception.Message);
        Assert.Equal(500, exception.StatusCode);
    }

    [Fact]
    public void ApiException_WithMessageAndStatusCode_SetsCorrectValues()
    {
        // Arrange & Act
        var exception = new ApiException("Test error", 400);

        // Assert
        Assert.Equal("Test error", exception.Message);
        Assert.Equal(400, exception.StatusCode);
    }

    [Fact]
    public void ResourceNotFoundException_Sets404StatusCode()
    {
        // Arrange & Act
        var exception = new ResourceNotFoundException("Resource not found");

        // Assert
        Assert.Equal("Resource not found", exception.Message);
        Assert.Equal(404, exception.StatusCode);
    }

    [Fact]
    public void ValidationException_Sets400StatusCodeAndErrors()
    {
        // Arrange
        var errors = new Dictionary<string, string[]>
        {
            { "Name", new[] { "Name is required" } },
            { "Age", new[] { "Age must be positive" } }
        };

        // Act
        var exception = new ValidationException("Validation failed", errors);

        // Assert
        Assert.Equal("Validation failed", exception.Message);
        Assert.Equal(400, exception.StatusCode);
        Assert.Equal(2, exception.Errors.Count);
        Assert.Contains("Name", exception.Errors.Keys);
    }

    [Fact]
    public void ValidationException_WithoutErrors_CreatesEmptyErrorsDictionary()
    {
        // Arrange & Act
        var exception = new ValidationException("Validation failed");

        // Assert
        Assert.Empty(exception.Errors);
    }

    [Fact]
    public void UnauthorizedException_Sets401StatusCode()
    {
        // Arrange & Act
        var exception = new UnauthorizedException("Invalid credentials");

        // Assert
        Assert.Equal("Invalid credentials", exception.Message);
        Assert.Equal(401, exception.StatusCode);
    }

    [Fact]
    public void UnauthorizedException_WithDefaultMessage_SetsDefaultMessage()
    {
        // Arrange & Act
        var exception = new UnauthorizedException();

        // Assert
        Assert.Equal("Unauthorized access", exception.Message);
        Assert.Equal(401, exception.StatusCode);
    }

    [Fact]
    public void ForbiddenException_Sets403StatusCode()
    {
        // Arrange & Act
        var exception = new ForbiddenException("Access denied");

        // Assert
        Assert.Equal("Access denied", exception.Message);
        Assert.Equal(403, exception.StatusCode);
    }

    [Fact]
    public void ForbiddenException_WithDefaultMessage_SetsDefaultMessage()
    {
        // Arrange & Act
        var exception = new ForbiddenException();

        // Assert
        Assert.Equal("Access forbidden", exception.Message);
        Assert.Equal(403, exception.StatusCode);
    }
}
