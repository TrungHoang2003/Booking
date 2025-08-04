using BuildingBlocks.Commons;
using BuildingBlocks.Interfaces;
using BuildingBlocks.Middlewares;
using BuildingBlocks.Services;
using DotNetEnv;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Orchestrator.Api.Database;
using Orchestrator.Api.Interfaces;
using Orchestrator.Api.Sagas;
using Orchestrator.Api.Services;
using Scalar.AspNetCore;
using Serilog;
using Sprache;
using StackExchange.Redis;

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
builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddScoped<IBecomeHostDraftService, BecomeHostDraftService>();
builder.Services.AddControllers();

// ConnectionStrings
var redisConnectionString = builder.Configuration["ConnectionStrings:Redis"];
var postgresConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    if (string.IsNullOrEmpty(redisConnectionString))
    {
        Log.Error("Redis connection string is null or empty");
        throw new ArgumentNullException(nameof(redisConnectionString), "Redis connection string cannot be null or empty");
    }
            
    var configOptions = ConfigurationOptions.Parse(redisConnectionString);
    return ConnectionMultiplexer.Connect(configOptions);
});

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
    // Đăng ký Saga
    busConfigurator.AddSagaStateMachine<BecomeHostSaga, BecomeHostSagaData>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic; // hoặc Optimistic
            r.AddDbContext<DbContext, SagaDbContext>((provider, optionsBuilder) =>
            {
                optionsBuilder.UseNpgsql(postgresConnectionString);
            });
        });
    
    
    busConfigurator.UsingRabbitMq((context, cfg) =>
    {
        var settings = context.GetRequiredService<MessageBrokerSettings>();
        
        cfg.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });
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