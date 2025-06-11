using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Middlewares;
public class ExceptionHandlerMiddleWare(RequestDelegate next)
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }

        catch (Exception ex)
        {
            var statusCode = ex switch
            {
                UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
                ArgumentException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
           
            httpContext.Response.StatusCode = statusCode;
            httpContext.Response.ContentType= "application/json";          
            var response = new ExceptionResponse
            {
                Description = ex.Message,
                StackTrace = ex.StackTrace
            };

            var json = JsonSerializer.Serialize(response);
            await httpContext.Response.WriteAsync(json);
        }
    }
}

public class ExceptionResponse
{
    public required string Description { get; set; }
    public string Code { get; set; } = "Internal Server Error";
    public string? StackTrace { get; set; }
}