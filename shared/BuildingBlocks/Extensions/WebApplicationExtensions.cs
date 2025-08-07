using BuildingBlocks.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Scalar.AspNetCore;
using Serilog;

namespace BuildingBlocks.Extensions;

public static class WebApplicationExtensions
{
    /// <summary>
    /// Configure common development middleware (OpenAPI, Scalar)
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <returns>WebApplication for chaining</returns>
    public static WebApplication UseCommonDevelopmentMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        return app;
    }

    /// <summary>
    /// Configure common middleware pipeline for APIs
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <param name="useTokenValidation">Whether to use token validation middleware</param>
    /// <param name="useCors">Whether to use CORS with AllowFrontend policy</param>
    /// <returns>WebApplication for chaining</returns>
    public static WebApplication UseCommonMiddleware(this WebApplication app, bool useTokenValidation = false, bool useCors = true)
    {
        app.UseRouting();
        
        if (useCors)
        {
            app.UseCors("AllowFrontend");
        }
        
        app.UseAuthentication();
        
        if (useTokenValidation)
        {
            // Uncomment when TokenValidateMiddleware is ready
            // app.UseMiddleware<TokenValidateMiddleware>();
        }
        
        app.UseAuthorization();
        app.UseMiddleware<ExceptionHandlerMiddleWare>();
        app.UseHttpsRedirection();
        app.UseSerilogRequestLogging();
        app.MapControllers();

        return app;
    }

    /// <summary>
    /// Run application with standard logging and error handling
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <param name="serviceName">Name of the service for logging</param>
    /// <param name="connectionStrings">Connection strings to log (optional)</param>
    /// <param name="environment">Environment name to log (optional)</param>
    public static void RunWithLogging(
        this WebApplication app, 
        string serviceName,
        Dictionary<string, string?>? connectionStrings = null,
        string? environment = null)
    {
        try
        {
            Log.Information("Starting {ServiceName}...", serviceName);
            
            if (connectionStrings?.Any() == true)
            {
                foreach (var connStr in connectionStrings)
                {
                    Log.Information("Using {ConnectionType}: {ConnectionString}", connStr.Key, connStr.Value);
                }
            }

            if (!string.IsNullOrEmpty(environment))
            {
                Log.Information("Environment: {Environment}", environment);
            }

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "{ServiceName} start-up failed", serviceName);
            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
