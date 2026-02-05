namespace AiRecipeGenerator.API.Authentication;

public class RoleMiddleware(RequestDelegate next)
{
    private const string RoleHeaderName = "X-User-Role";

    public async Task InvokeAsync(HttpContext context)
    {
        var roleHeader = context.Request.Headers[RoleHeaderName].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(roleHeader) && Enum.TryParse<UserRole>(roleHeader, ignoreCase: true, out var role))
        {
            context.Items["UserRole"] = role;
        }
        else
        {
            context.Items["UserRole"] = UserRole.User;
        }

        await next(context);
    }
}
