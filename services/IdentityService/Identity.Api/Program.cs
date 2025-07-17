using BuildingBlocks.Commons;
using BuildingBlocks.Middlewares;
using Identity.Api.Middlewares;
using Identity.Application;
using Identity.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Exceptions;
using DotNetEnv;
using MassTransit;
using Microsoft.Extensions.Options;

// Load environment-specific .env file
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var envFile = environment switch
{
    "Development" => ".env.local",
    "Docker" => ".env.docker",
    "Production" => ".env.production",
    _ => ".env.local"
};

var envPath = Path.Combine(Directory.GetCurrentDirectory(),envFile);
if (File.Exists(envPath))
{
    Env.Load(envPath);
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddMassTransit(busConfigurator =>
{
    // Register consumers
    
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        var settings = context.GetRequiredService<MessageBrokerSettings>();
        
        configurator.Host(new Uri(settings.Host), h =>
        {
           h.Username(settings.Username); 
           h.Password(settings.Password);
        });
        
        // Configure endpoints for consumers
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddOpenApi();

// Get connection string from configuration
var postgresConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];
var redisConnectionString = builder.Configuration["ConnectionStrings:Redis"];

// Add Infrastructure layer with PostgreSQL configuration
builder.Services.AddInfrastructure(postgresConnectionString, redisConnectionString);

// Add Application layer
builder.Services.AddApplication();

// Add Controllers
builder.Services.AddControllers();

// Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration) // Đọc cấu hình từ appsettings.json
    .ReadFrom.Services(services) // Cho phép DI cho các enricher/sinks nếu cần
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<ExceptionHandlerMiddleWare>();
//app.UseMiddleware<TokenValidateMiddleware>();
app.UseHttpsRedirection();
// Add Serilog request logging
app.UseSerilogRequestLogging();
app.MapControllers();

try
{
    Log.Information("Starting Identity Service...");
    Log.Information("Using connection string: {ConnectionString}", postgresConnectionString);
    Log.Information("Using Redis connection string: {RedisConnectionString}", redisConnectionString);
    Log.Information("Environment: {Environment}", environment);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Identity Service start-up failed");
    throw;
}
finally
{
    Log.CloseAndFlush(); //Đảm bảo tất cả các log đang chờ được đẩy đi trước khi ứng dụng tắt
}

