using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Text.Json;
using BuildingBlocks.Commons;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.Middlewares;
public class ExceptionHandlerMiddleWare(RequestDelegate next, ILogger<ExceptionHandlerMiddleWare> logger)
{
   public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, error) = exception switch
        {
            BusinessException businessEx => (StatusCodes.Status400BadRequest, 
                new Error(businessEx.ErrorCode, businessEx.Message)),
            ValidationException validationEx => (StatusCodes.Status400BadRequest, 
                new Error("VALIDATION_ERROR", $"One or more validation errors occurred: {validationEx.Errors}")),
            UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, 
                new Error("UNAUTHORIZED", "Access denied")),
            ArgumentException argEx => (StatusCodes.Status400BadRequest, 
                new Error("INVALID_ARGUMENT", argEx.Message)),
            KeyNotFoundException => (StatusCodes.Status404NotFound, 
                new Error("NOT_FOUND", "The requested resource was not found")),
            TimeoutException => (StatusCodes.Status408RequestTimeout, 
                new Error("REQUEST_TIMEOUT", "The request timed out")),
            _ => (StatusCodes.Status500InternalServerError, 
                new Error("INTERNAL_SERVER_ERROR", "An internal server error occurred"))
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        
        var result = Result.Failure(error);
        
        var response = new
        {
            result.Success,
            Error = new
            {
                result.Error?.Code,
                result.Error?.Description,
                StackTrace = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" 
                    ? exception.StackTrace : null
            }
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}

public class ValidationException : Exception
{
    public Dictionary<string, string[]> Errors { get; }
    
    public ValidationException(Dictionary<string, string[]> errors) : base("Validation failed")
    {
        Errors = errors;
    }
}

public class BusinessException : Exception
{
    public string ErrorCode { get; }
    
    public BusinessException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }
}