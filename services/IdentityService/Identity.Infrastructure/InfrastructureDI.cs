using Identity.Domain.Entities;
using Identity.Infrastructure.DbHelper;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Serilog;
using SqlKata.Compilers;
using StackExchange.Redis;

namespace Identity.Infrastructure;

public static class InfrastructureDi
{
    public static void AddInfrastructure(this IServiceCollection services, string? postgresConnectionString, string? redisConnectionString)
    {
        Log.Information("Using connection string: {ConnectionString}", postgresConnectionString);

        try
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(postgresConnectionString));
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to configure ApplicationDbContext: {ErrorMessage}", ex.Message);
            throw;
        }
        
        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }).AddEntityFrameworkStores<IdentityDbContext>();
        
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

        services.AddSingleton<HttpClient>();
        services.AddSingleton<PostgresCompiler>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
    }
}