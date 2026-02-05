using System.Net;

using AiRecipeGenerator.API.Exceptions;
using AiRecipeGenerator.API.Models.Responses;
using AiRecipeGenerator.Application.Exceptions;

namespace AiRecipeGenerator.API.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            Timestamp = DateTime.UtcNow,
            TraceId = context.TraceIdentifier
        };

        switch (exception)
        {
            case InvalidRecipeContentException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = ex.Message;
                errorResponse.ErrorType = nameof(InvalidRecipeContentException);
                break;

            case ValidationException ex:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.Message = ex.Message;
                errorResponse.ErrorType = nameof(ValidationException);
                errorResponse.ValidationErrors = ex.Errors;
                break;

            case ResourceNotFoundException ex:
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.Message = ex.Message;
                errorResponse.ErrorType = nameof(ResourceNotFoundException);
                break;

            case UnauthorizedException ex:
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.Message = ex.Message;
                errorResponse.ErrorType = nameof(UnauthorizedException);
                break;

            case ForbiddenException ex:
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                errorResponse.StatusCode = (int)HttpStatusCode.Forbidden;
                errorResponse.Message = ex.Message;
                errorResponse.ErrorType = nameof(ForbiddenException);
                break;

            case ApiException ex:
                context.Response.StatusCode = ex.StatusCode;
                errorResponse.StatusCode = ex.StatusCode;
                errorResponse.Message = ex.Message;
                errorResponse.ErrorType = nameof(ApiException);
                break;

            default:
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.Message = "An internal server error occurred";
                errorResponse.ErrorType = exception.GetType().Name;
                break;
        }

        return context.Response.WriteAsJsonAsync(errorResponse);
    }
}
