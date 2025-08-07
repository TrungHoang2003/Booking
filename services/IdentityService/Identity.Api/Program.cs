using BuildingBlocks.Commons;
using BuildingBlocks.Extensions;
using BuildingBlocks.Middlewares;
using Identity.Application;
using Identity.Infrastructure;
using Identity.Application.Consumers;
using DotNetEnv;

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

// Add MessageBroker configuration
builder.Services.AddMessageBrokerSettings(builder.Configuration);

// Add MassTransit with RabbitMQ
builder.Services.AddMassTransitWithRabbitMQ(busConfigurator =>
{
    // Register consumers
    busConfigurator.AddConsumer<UpdateHostProfileConsumer>();
});

// Get connection strings from configuration
var postgresConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
var redisConnectionString = builder.Configuration["ConnectionStrings:Redis"];

// Add Infrastructure and Application layers
builder.Services.AddInfrastructure(postgresConnectionString, redisConnectionString);
builder.Services.AddApplication();

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

app.RunWithLogging("Identity Service", connectionStrings, environment);

