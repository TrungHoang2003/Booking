using Microsoft.AspNetCore.Builder;

namespace BuildingBlocks.Extensions;

public static class EnvironmentExtensions
{
    /// <summary>
    /// Get MessageBroker settings from environment variables or configuration
    /// </summary>
    /// <param name="builder">WebApplication builder</param>
    /// <returns>MessageBroker settings values</returns>
    public static (string Host, string Username, string Password) GetMessageBrokerSettings(this WebApplicationBuilder builder)
    {
        var host = Environment.GetEnvironmentVariable("MessageBroker__Host") 
                   ?? builder.Configuration["MessageBroker:Host"] 
                   ?? "localhost";
                   
        var username = Environment.GetEnvironmentVariable("MessageBroker__Username") 
                       ?? builder.Configuration["MessageBroker:Username"] 
                       ?? "guest";
                       
        var password = Environment.GetEnvironmentVariable("MessageBroker__Password") 
                       ?? builder.Configuration["MessageBroker:Password"] 
                       ?? "guest";

        return (host, username, password);
    }
}
