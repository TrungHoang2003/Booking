using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BuildingBlocks.Extensions;

public static class MinimalApiExtensions
{
    /// <summary>
    /// Add minimal API services (Swagger, Controllers, etc.)
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddMinimalApiServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddControllers();
        
        return services;
    }

    /// <summary>
    /// Add minimal API services with CORS support
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="allowedOrigins">Optional custom CORS origins. Defaults to localhost:3000</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddMinimalApiServicesWithCors(
        this IServiceCollection services,
        params string[] allowedOrigins)
    {
        services.AddMinimalApiServices();
        services.AddCorsConfiguration(allowedOrigins);
        
        return services;
    }

    /// <summary>
    /// Configure minimal API pipeline for simple services
    /// </summary>
    /// <param name="app">WebApplication</param>
    /// <param name="useCors">Whether to use CORS. Default is true</param>
    /// <returns>WebApplication for chaining</returns>
    public static WebApplication UseMinimalApiPipeline(this WebApplication app, bool useCors = true)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        if (useCors)
        {
            app.UseCors("AllowFrontend");
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        return app;
    }

    /// <summary>
    /// Run minimal application without advanced logging
    /// </summary>
    /// <param name="app">WebApplication</param>
    public static void RunMinimal(this WebApplication app)
    {
        app.Run();
    }
}
