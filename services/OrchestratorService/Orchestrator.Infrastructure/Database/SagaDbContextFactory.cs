using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Orchestrator.Infrastructure.Database;

public class SagaDbContextFactory : IDesignTimeDbContextFactory<SagaDbContext>
{
    public SagaDbContext CreateDbContext(string[] args)
    {
        // Load file .env.local vào biến môi trường
        Env.Load(".env.local");
        
        var optionsBuilder = new DbContextOptionsBuilder<SagaDbContext>();

        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("Missing connection string from .env.local or environment.");
        }        optionsBuilder.UseNpgsql(connectionString);

        return new SagaDbContext(optionsBuilder.Options);
    }
}
