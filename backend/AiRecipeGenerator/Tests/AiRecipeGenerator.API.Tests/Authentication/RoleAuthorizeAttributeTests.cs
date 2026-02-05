using AiRecipeGenerator.API.Authentication;
using AiRecipeGenerator.API.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Xunit;

namespace AiRecipeGenerator.API.Tests.Authentication;

public class RoleAuthorizeAttributeTests
{
    [Fact]
    public void OnAuthorization_WithAllowedRole_DoesNotThrow()
    {
        // Arrange
        var attribute = new RoleAuthorizeAttribute(UserRole.User);
        var httpContext = new DefaultHttpContext();
        httpContext.Items["UserRole"] = UserRole.User;

        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
        var authorizationFilterContext = new AuthorizationFilterContext(actionContext, []);

        // Act & Assert
        // Should not throw
        attribute.OnAuthorization(authorizationFilterContext);
    }

    [Fact]
    public void OnAuthorization_WithDisallowedRole_ThrowsForbiddenException()
    {
        // Arrange
        var attribute = new RoleAuthorizeAttribute(UserRole.Admin);
        var httpContext = new DefaultHttpContext();
        httpContext.Items["UserRole"] = UserRole.User;

        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
        var authorizationFilterContext = new AuthorizationFilterContext(actionContext, []);

        // Act & Assert
        Assert.Throws<ForbiddenException>(() => attribute.OnAuthorization(authorizationFilterContext));
    }

    [Fact]
    public void OnAuthorization_WithMultipleAllowedRoles_AllowsMatchingRole()
    {
        // Arrange
        var attribute = new RoleAuthorizeAttribute(UserRole.User, UserRole.Admin);
        var httpContext = new DefaultHttpContext();
        httpContext.Items["UserRole"] = UserRole.Admin;

        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
        var authorizationFilterContext = new AuthorizationFilterContext(actionContext, []);

        // Act
        attribute.OnAuthorization(authorizationFilterContext);

        // Assert
        Assert.Null(authorizationFilterContext.Result);
    }

    [Fact]
    public void OnAuthorization_WithMissingRoleInContext_ThrowsForbiddenException()
    {
        // Arrange
        var attribute = new RoleAuthorizeAttribute(UserRole.User);
        var httpContext = new DefaultHttpContext();

        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
        var authorizationFilterContext = new AuthorizationFilterContext(actionContext, []);

        // Act & Assert
        Assert.Throws<ForbiddenException>(() => attribute.OnAuthorization(authorizationFilterContext));
    }

    [Fact]
    public void OnAuthorization_WithoutSpecifiedRoles_DefaultsToUserRole()
    {
        // Arrange
        var attribute = new RoleAuthorizeAttribute();
        var httpContext = new DefaultHttpContext();
        httpContext.Items["UserRole"] = UserRole.User;

        var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor());
        var authorizationFilterContext = new AuthorizationFilterContext(actionContext, []);

        // Act & Assert
        // Should not throw when user has User role
        attribute.OnAuthorization(authorizationFilterContext);
    }
}
