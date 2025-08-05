using BuildingBlocks.Commons;
using BuildingBlocks.Middlewares;
using DotNetEnv;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Orchestrator.Application;
using Orchestrator.Application.Sagas;
using Orchestrator.Domain.Models;
using Orchestrator.Infrastructure;
using Orchestrator.Infrastructure.Database;
using Scalar.AspNetCore;
using Serilog;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
var enFile = environment switch
{
    "Development" => ".env.local",
    "Docker" => ".env.docker",
    "Production" => ".env.production",
    _ => ".env.local"
};

var envPath = Path.Combine(Directory.GetCurrentDirectory(), enFile);
if (File.Exists(envPath))
{
    Env.Load(envPath);
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();

// ConnectionStrings
var redisConnectionString = builder.Configuration["ConnectionStrings:Redis"];
var postgresConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];


// Add Infrastructure layer with PostgresSQL configuration
builder.Services.AddInfrastructure(redisConnectionString, postgresConnectionString);

// Add Application layer
builder.Services.AddApplication();

// Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration) // Đọc cấu hình từ appsettings.json
        .ReadFrom.Services(services) // Cho phép DI cho các enricher/sinks nếu cần
);

// Lấy section "MessageBroker" từ file appsettings.json và bind nó vào class MessageBrokerSettings.
builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

// Đăng ký MessageBrokerSettings 
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

// Cấu hình và khởi tạo MassTransit dùng RabbitMQ
builder.Services.AddMassTransit(busConfigurator =>
{
    // Đăng ký consumers
    busConfigurator.AddConsumers(typeof(Program).Assembly);
    
    // Đăng ký Saga - sử dụng DbContext đã được đăng ký trong Infrastructure
    busConfigurator.AddSagaStateMachine<BecomeHostSaga, BecomeHostSagaData>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // hoặc Optimistic

            r.UsePostgres();
            
            r.ExistingDbContext<Orchestrator.Infrastructure.Database.SagaDbContext>();
        });
    
    busConfigurator.UsingRabbitMq((context, cfg) =>
    {
        var settings = context.GetRequiredService<MessageBrokerSettings>();
        
        cfg.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });
        
        // Configure endpoints for consumers and sagas
        cfg.ConfigureEndpoints(context);
    }); 
});

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
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.MapControllers();

try
{
    Log.Information("Starting Orchestrator Service...");
    Log.Information("Using connection string: {postgresConnectionString}",postgresConnectionString );
    Log.Information("Using Redis connection string: {RedisConnectionString}", redisConnectionString);
    Log.Information("Environment: {Environment}", environment);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}