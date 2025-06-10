using Identity.Domain.Entities;
using Identity.Infrastructure.DbHelper;
using Identity.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SqlKata.Compilers;
using StackExchange.Redis;

namespace Identity.Infrastructure;

public static class InfrastructureDi
{
    public static void AddInfrastructure(this IServiceCollection services, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            Console.WriteLine("Connection string is not set in environment variables.");
            throw new Exception("Connection string has not been set up");
        }

        Console.WriteLine($"Using connection string: {connectionString}");

        try
        {
            services.AddDbContext<IdentityDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to configure ApplicationDbContext: {ex.Message}");
            throw;
        }        services.AddIdentity<User, IdentityRole<int>>(options =>
        {
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        }).AddEntityFrameworkStores<IdentityDbContext>();

        // Add Redis connection
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configuration = ConfigurationOptions.Parse("localhost:6379");
            return ConnectionMultiplexer.Connect(configuration);
        });

        services.AddSingleton<HttpClient>();
        services.AddSingleton<PostgresCompiler>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
    }
}