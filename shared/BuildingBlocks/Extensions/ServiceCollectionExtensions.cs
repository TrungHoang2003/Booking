using BuildingBlocks.Commons;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Serilog;

namespace BuildingBlocks.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Add and configure MassTransit with RabbitMQ using MessageBrokerSettings from configuration
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configure">Action to configure MassTransit (register consumers, sagas, etc.)</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddMassTransitWithRabbitMQ(
        this IServiceCollection services, 
        Action<IBusRegistrationConfigurator> configure)
    {
        services.AddMassTransit(busConfigurator =>
        {
            configure(busConfigurator);
            
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                var settings = context.GetRequiredService<MessageBrokerSettings>();
                
                configurator.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });
                
                configurator.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    /// <summary>
    /// Add MessageBroker settings configuration
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddMessageBrokerSettings(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
        
        return services;
    }

    /// <summary>
    /// Add Serilog configuration using standard pattern
    /// </summary>
    /// <param name="builder">WebApplication builder</param>
    /// <returns>WebApplication builder for chaining</returns>
    public static WebApplicationBuilder AddSerilogConfiguration(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services));

        return builder;
    }

    /// <summary>
    /// Add common API services (Controllers, OpenAPI, etc.)
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddCommonApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddHttpContextAccessor();
        
        return services;
    }

    /// <summary>
    /// Add CORS configuration for local development with Next.js frontend
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="allowedOrigins">Optional custom origins. Defaults to localhost:3000</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddCorsConfiguration(
        this IServiceCollection services, 
        params string[] allowedOrigins)
    {
        var origins = allowedOrigins?.Length > 0 ? allowedOrigins : new[] { "http://localhost:3000", "https://localhost:3000" };

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(origins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        return services;
    }

    /// <summary>
    /// Add all common API services including CORS
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="allowedOrigins">Optional custom CORS origins. Defaults to localhost:3000</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddCommonApiServicesWithCors(
        this IServiceCollection services,
        params string[] allowedOrigins)
    {
        services.AddCommonApiServices();
        services.AddCorsConfiguration(allowedOrigins);
        
        return services;
    }

    /// <summary>
    /// Add MessageBroker settings with environment variable support
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="builder">WebApplication builder for accessing environment variables</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddMessageBrokerSettingsFromEnvironment(
        this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        var (host, username, password) = builder.GetMessageBrokerSettings();
        
        var messageBrokerSettings = new MessageBrokerSettings
        {
            Host = host,
            Username = username,
            Password = password
        };

        services.AddSingleton(messageBrokerSettings);
        
        return services;
    }
}
