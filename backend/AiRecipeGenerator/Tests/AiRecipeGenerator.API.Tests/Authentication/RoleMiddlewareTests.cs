using AiRecipeGenerator.API.Authentication;

using Microsoft.AspNetCore.Http;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Authentication;

public class RoleMiddlewareTests
{
    [Theory]
    [InlineData("User")]
    [InlineData("user")]
    [InlineData("Admin")]
    [InlineData("admin")]
    public async Task InvokeAsync_WithValidRoleHeader_SetsUserRoleInContext(string roleValue)
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-User-Role"] = roleValue;

        var nextCalled = false;
        RequestDelegate next = async context =>
        {
            nextCalled = true;
            await Task.CompletedTask;
        };

        var middleware = new RoleMiddleware(next);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.True(nextCalled);
        Assert.True(httpContext.Items.ContainsKey("UserRole"));
        Assert.IsType<UserRole>(httpContext.Items["UserRole"]);
    }

    [Fact]
    public async Task InvokeAsync_WithMissingRoleHeader_DefaultsToUserRole()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();

        var nextCalled = false;
        RequestDelegate next = async context =>
        {
            nextCalled = true;
            await Task.CompletedTask;
        };

        var middleware = new RoleMiddleware(next);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.True(nextCalled);
        Assert.True(httpContext.Items.ContainsKey("UserRole"));
        Assert.Equal(UserRole.User, httpContext.Items["UserRole"]);
    }

    [Fact]
    public async Task InvokeAsync_WithInvalidRoleHeader_DefaultsToUserRole()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-User-Role"] = "InvalidRole";

        var nextCalled = false;
        RequestDelegate next = async context =>
        {
            nextCalled = true;
            await Task.CompletedTask;
        };

        var middleware = new RoleMiddleware(next);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal(UserRole.User, httpContext.Items["UserRole"]);
    }

    [Fact]
    public async Task InvokeAsync_WithEmptyRoleHeader_DefaultsToUserRole()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["X-User-Role"] = string.Empty;

        var nextCalled = false;
        RequestDelegate next = async context =>
        {
            nextCalled = true;
            await Task.CompletedTask;
        };

        var middleware = new RoleMiddleware(next);

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.True(nextCalled);
        Assert.Equal(UserRole.User, httpContext.Items["UserRole"]);
    }
}
