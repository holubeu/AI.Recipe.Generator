namespace AiRecipeGenerator.Application.Exceptions;

public class InvalidRecipeContentException : Exception
{
    public InvalidRecipeContentException(string message) : base(message)
    {
    }

    public InvalidRecipeContentException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
