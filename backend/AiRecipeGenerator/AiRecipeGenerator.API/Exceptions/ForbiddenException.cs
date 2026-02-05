namespace AiRecipeGenerator.API.Exceptions;

public class ForbiddenException : ApiException
{
    public ForbiddenException(string message = "Access forbidden") : base(message, 403)
    {
    }
}
