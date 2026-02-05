namespace AiRecipeGenerator.API.Exceptions;

public class ResourceNotFoundException : ApiException
{
    public ResourceNotFoundException(string message) : base(message, 404)
    {
    }
}
