using AiRecipeGenerator.API.Exceptions;

using Microsoft.AspNetCore.Mvc.Filters;

namespace AiRecipeGenerator.API.Authentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RoleAuthorizeAttribute(params UserRole[] allowedRoles) : Attribute, IAuthorizationFilter
{
    private readonly UserRole[] _allowedRoles = allowedRoles.Length > 0 ? allowedRoles : [UserRole.User];

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Items.TryGetValue("UserRole", out var userRoleObj) && userRoleObj is UserRole userRole)
        {
            if (_allowedRoles.Contains(userRole))
            {
                return;
            }
        }

        throw new ForbiddenException("Access forbidden");
    }
}
