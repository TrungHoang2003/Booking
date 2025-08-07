# BuildingBlocks Extensions

This document explains how to use the shared extension methods in the BuildingBlocks library to reduce code duplication across microservices.

## Available Extensions

### 1. EnvironmentExtensions

**`GetConnectionString()`** - Get connection string from environment variables or config
```csharp
var connectionString = builder.GetConnectionString("DefaultConnection");
// Prioritizes environment variables over appsettings.json
// Looks for: ConnectionStrings__DefaultConnection -> ConnectionStrings:DefaultConnection
```

**`GetConnectionStrings()`** - Get all common connection strings
```csharp
var connectionStrings = builder.GetConnectionStrings();
// Returns Dictionary with DefaultConnection and Redis
```

**`GetMessageBrokerSettings()`** - Get MessageBroker settings from environment
```csharp
var (host, username, password) = builder.GetMessageBrokerSettings();
// Looks for: MessageBroker__Host, MessageBroker__Username, MessageBroker__Password
```

### 2. ServiceCollectionExtensions

**`AddCommonApiServices()`** - Add standard API services
```csharp
builder.Services.AddCommonApiServices(); // Controllers, OpenAPI, HttpContextAccessor
```

**`AddCorsConfiguration()`** - Configure CORS for frontend communication
```csharp
builder.Services.AddCorsConfiguration(); // Default: localhost:3000
// or with custom origins:
builder.Services.AddCorsConfiguration("http://localhost:3001", "https://mydomain.com");
```

**`AddCommonApiServicesWithCors()`** - Add API services + CORS in one call
```csharp
builder.Services.AddCommonApiServicesWithCors(); // Includes Controllers, OpenAPI, CORS
```

**`AddMessageBrokerSettings()`** - Configure MessageBroker settings
```csharp
builder.Services.AddMessageBrokerSettings(builder.Configuration);
```

**`AddMessageBrokerSettingsFromEnvironment()`** - Configure MessageBroker from env vars
```csharp
builder.Services.AddMessageBrokerSettingsFromEnvironment(builder);
// Prioritizes environment variables over configuration files
```

**`AddMassTransitWithRabbitMQ()`** - Configure MassTransit with RabbitMQ
```csharp
builder.Services.AddMassTransitWithRabbitMQ(busConfigurator =>
{
    busConfigurator.AddConsumer<YourConsumer>();
});
```

**`AddSerilogConfiguration()`** - Configure Serilog with standard pattern
```csharp
builder.AddSerilogConfiguration();
```

### 3. MassTransitSagaExtensions

**`AddMassTransitWithRabbitMQAndSaga()`** - Configure MassTransit with Saga support
```csharp
builder.Services.AddMassTransitWithRabbitMQAndSaga<YourDbContext>(busConfigurator =>
{
    busConfigurator.AddSagaWithEntityFramework<YourSaga, YourSagaData, YourDbContext>();
});
```

### 4. WebApplicationExtensions

**`UseCommonDevelopmentMiddleware()`** - Configure development middleware
```csharp
app.UseCommonDevelopmentMiddleware(); // OpenAPI, Scalar in Development
```

**`UseCommonMiddleware()`** - Configure standard middleware pipeline
```csharp
app.UseCommonMiddleware(); // Routing, Auth, CORS, Exception handling, etc.
// or disable CORS:
app.UseCommonMiddleware(useCors: false);
// or disable CORS and enable token validation:
app.UseCommonMiddleware(useTokenValidation: true, useCors: false);
```

**`RunWithLogging()`** - Run with standardized logging and error handling
```csharp
var connectionStrings = new Dictionary<string, string?>
{
    ["DefaultConnection"] = postgresConnectionString,
    ["Redis"] = redisConnectionString
};

app.RunWithLogging("Your Service Name", connectionStrings, environment);
```

### 5. MinimalApiExtensions

For simple services without complex requirements:

```csharp
using BuildingBlocks.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Basic minimal services
builder.Services.AddMinimalApiServices();
// or with CORS:
builder.Services.AddMinimalApiServicesWithCors();

var app = builder.Build();

// Basic pipeline
app.UseMinimalApiPipeline()
   .RunMinimal();
// or disable CORS:
app.UseMinimalApiPipeline(useCors: false)
   .RunMinimal();
```

## Usage Examples

### Full-Featured Service (Identity, Property, Orchestrator)

```csharp
using BuildingBlocks.Extensions;
using YourService.Application;
using YourService.Infrastructure;
using DotNetEnv;

// Load environment file manually
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

// Add services with CORS
builder.Services.AddCommonApiServicesWithCors();
builder.Services.AddMessageBrokerSettingsFromEnvironment(builder);

// Configure MassTransit
builder.Services.AddMassTransitWithRabbitMQ(busConfigurator =>
{
    busConfigurator.AddConsumer<YourConsumer>();
});

// Add your layers with connection strings from environment
var connectionStrings = builder.GetConnectionStrings();
var connectionString = connectionStrings["DefaultConnection"];
builder.Services.AddInfrastructure(connectionString);
builder.Services.AddApplication();

// Add logging
builder.AddSerilogConfiguration();

var app = builder.Build();

// Configure pipeline (CORS enabled by default)
app.UseCommonDevelopmentMiddleware();
app.UseCommonMiddleware();

// Run with logging
app.RunWithLogging("Your Service", connectionStrings, environment);
```

### Saga-Based Service (Orchestrator)

```csharp
using BuildingBlocks.Extensions;
using MassTransit;

// ... (same setup as above)

// Configure MassTransit with Saga
builder.Services.AddMassTransitWithRabbitMQAndSaga<YourSagaDbContext>(busConfigurator =>
{
    busConfigurator.AddSagaWithEntityFramework<YourSaga, YourSagaData, YourSagaDbContext>(
        ConcurrencyMode.Pessimistic);
});

// ... (rest same as above)
```

### Simple Service (Booking, Gateway)

```csharp
using BuildingBlocks.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMinimalApiServices();

var app = builder.Build();
app.UseMinimalApiPipeline().RunMinimal();
```

## Benefits

1. **Reduced Code Duplication** - Common patterns extracted into reusable methods
2. **Consistency** - All services follow the same patterns
3. **Maintainability** - Changes to common logic only need to be made in one place
4. **Type Safety** - Strong typing with generics where appropriate
5. **Flexibility** - Extension methods can be chained and customized

## Dependencies Added to BuildingBlocks

The following packages were added to support these extensions:

- `DotNetEnv` - Environment file loading
- `Microsoft.AspNetCore.OpenApi` - OpenAPI support
- `Scalar.AspNetCore` - API documentation
- `Serilog.AspNetCore` - Logging
- `MassTransit.EntityFrameworkCore` - Saga persistence
- `Microsoft.EntityFrameworkCore` - Entity Framework support

## Environment Variables

The extension methods prioritize environment variables over configuration files. Use the following naming conventions in your .env files:

### Connection Strings
```bash
# .env.local, .env.docker, etc.
ConnectionStrings__DefaultConnection=Server=localhost;Database=YourDb;Username=postgres;Password=yourpassword
ConnectionStrings__Redis=localhost:6379
```

### MessageBroker Settings
```bash
MessageBroker__Host=amqp://localhost:5672
MessageBroker__Username=guest  
MessageBroker__Password=guest
```

### Example .env.local file
```bash
# Database
ConnectionStrings__DefaultConnection=Server=localhost;Database=BookingIdentity;Username=postgres;Password=postgres;
ConnectionStrings__Redis=localhost:6379

# RabbitMQ
MessageBroker__Host=amqp://localhost:5672
MessageBroker__Username=guest
MessageBroker__Password=guest

# Logging
ASPNETCORE_ENVIRONMENT=Development
```

Note: Use double underscores `__` for nested configuration keys in environment variables.
