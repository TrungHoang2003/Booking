using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Orchestrator.Infrastructure;

public class PostgresServer(IConfiguration configuration)
{
    public NpgsqlConnection OpenConnection()
    {
        string connectionString;
        
        connectionString = configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Postgres connection string not found");

        var cnn = new NpgsqlConnection(connectionString);

        if (cnn.State == ConnectionState.Closed)
        {
            cnn.Open();
        }

        return cnn;
    }
}