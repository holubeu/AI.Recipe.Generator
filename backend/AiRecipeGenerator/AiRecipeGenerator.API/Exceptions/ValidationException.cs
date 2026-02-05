namespace AiRecipeGenerator.API.Exceptions;

public class ValidationException : ApiException
{
    public Dictionary<string, string[]> Errors { get; set; }

    public ValidationException(string message, Dictionary<string, string[]> errors = null) : base(message, 400)
    {
        Errors = errors ?? new Dictionary<string, string[]>();
    }
}
