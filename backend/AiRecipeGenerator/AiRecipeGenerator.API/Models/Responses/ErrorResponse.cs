namespace AiRecipeGenerator.API.Models.Responses;

public class ErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string ErrorType { get; set; }
    public string TraceId { get; set; }
    public Dictionary<string, string[]> ValidationErrors { get; set; }
    public DateTime Timestamp { get; set; }
}
