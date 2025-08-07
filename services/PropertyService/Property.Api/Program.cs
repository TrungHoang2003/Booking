using BuildingBlocks.Extensions;
using Property.Application;
using Property.Application.Consumers;
using Property.Infrastructure;
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
    busConfigurator.AddConsumer<CreatePropertyConsumer>();
    busConfigurator.AddConsumer<AddRentalUnitConsumer>();
    busConfigurator.AddConsumer<AddBedroomConsumer>();
});

// Get connection strings from configuration
var postgresConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

// Add Infrastructure and Application layers
builder.Services.AddInfrastructure(postgresConnectionString);
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
    ["DefaultConnection"] = postgresConnectionString
};

app.RunWithLogging("Property Service", connectionStrings, environment);
