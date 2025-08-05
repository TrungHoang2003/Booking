using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orchestrator.Infrastructure.Database;
using Serilog;
using StackExchange.Redis;

namespace Orchestrator.Infrastructure;

public static class OrchestratorInfrastructureDi
{
    public static void AddInfrastructure(this IServiceCollection services, string? redisConnectionString, string? postgresConnectionString = null)
    {
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            if (string.IsNullOrEmpty(redisConnectionString))
            {
                Log.Error("Redis connection string is null or empty");
                throw new ArgumentNullException(nameof(redisConnectionString), "Redis connection string cannot be null or empty");
            }
            
            var configOptions = ConfigurationOptions.Parse(redisConnectionString);
            return ConnectionMultiplexer.Connect(configOptions);
        });

        Log.Information("Using connection string: {ConnectionString}", postgresConnectionString);

        try
        {
            services.AddDbContext<SagaDbContext>(options =>
                options.UseNpgsql(postgresConnectionString));
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to configure ApplicationDbContext: {ErrorMessage}", ex.Message);
            throw;
        }

        services.AddSingleton<HttpClient>();
    }
}