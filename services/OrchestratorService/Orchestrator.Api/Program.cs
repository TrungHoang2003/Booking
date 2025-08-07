using BuildingBlocks.Extensions;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Orchestrator.Application;
using Orchestrator.Application.Sagas;
using Orchestrator.Domain.Models;
using Orchestrator.Infrastructure;
using DotNetEnv;
using OrchestratorSagaDbContext = Orchestrator.Infrastructure.Database.SagaDbContext;

// Load environment-specific .env file
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var envFile = environment switch
{
    "Development" => ".env.local",
    "Docker" => ".env.docker",
    "Production" => ".env.production",
    _ => ".env.local"
};

var envPath = Path.Combine(Directory.GetCurrentDirectory(), envFile);
if (File.Exists(envPath))
{
    Env.Load(envPath);
}

var builder = WebApplication.CreateBuilder(args);

// Add common API services with CORS
builder.Services.AddCommonApiServicesWithCors();

// Get connection strings from configuration
var redisConnectionString = builder.Configuration["ConnectionStrings:Redis"];
var postgresConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

// Add Infrastructure and Application layers
builder.Services.AddInfrastructure(redisConnectionString, postgresConnectionString);
builder.Services.AddApplication();

// Add MessageBroker configuration
builder.Services.AddMessageBrokerSettings(builder.Configuration);

// Add MassTransit with RabbitMQ and Saga support
builder.Services.AddMassTransitWithRabbitMqAndSaga<OrchestratorSagaDbContext>(busConfigurator =>
{
    // Register Saga with Entity Framework
    busConfigurator.AddSagaWithEntityFramework<BecomeHostSaga, BecomeHostSagaData, OrchestratorSagaDbContext>(
        ConcurrencyMode.Pessimistic);
});

// Add Serilog
builder.AddSerilogConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCommonDevelopmentMiddleware();
app.UseCommonMiddleware();

// Run with logging
var connectionStrings = new Dictionary<string, string?>
{
    ["DefaultConnection"] = postgresConnectionString,
    ["Redis"] = redisConnectionString
};

app.RunWithLogging("Orchestrator Service", connectionStrings, environment);