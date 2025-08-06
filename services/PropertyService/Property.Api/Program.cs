using BuildingBlocks.Commons;
using BuildingBlocks.Middlewares;
using Contracts.Messages;
using DotNetEnv;
using MassTransit;
using Microsoft.Extensions.Options;
using Property.Api;
using Property.Application;
using Property.Application.Consumers;
using Property.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using Sprache;

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

// Lấy section "MessageBroker" từ file appsettings.json và bind nó vào class MessageBrokerSettings.
builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));

// Đăng ký MessageBrokerSettings 
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

// Cấu hình và khởi tạo MassTransit dùng RabbitMQ
builder.Services.AddMassTransit(busConfigurator =>
{
    // Đăng ký consumers
    busConfigurator.AddConsumer<CreatePropertyConsumer>();
    busConfigurator.AddConsumer<AddRentalUnitConsumer>();
    busConfigurator.AddConsumer<AddBedroomConsumer>();
    
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

builder.Services.AddOpenApi();
// Get connection string from configuration
var postgresConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

// Add Infrastructure layer with PostgresSQL configuration
builder.Services.AddInfrastructure(postgresConnectionString);

// Add Application layer
 builder.Services.AddApplication();

// Add Controllers
builder.Services.AddControllers();

// Add HttpContextAccessor for accessing current user
builder.Services.AddHttpContextAccessor();

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
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleWare>();
//app.UseMiddleware<TokenValidateMiddleware>();
app.UseHttpsRedirection();
// Add Serilog request logging
app.UseSerilogRequestLogging();
app.MapControllers();

try
{
    Log.Information("Starting Property Service...");
    Log.Information("Using connection string: {ConnectionString}", postgresConnectionString);
    Log.Information("Environment: {Environment}", environment);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Property Service start-up failed");
    throw;
}
finally
{
    Log.CloseAndFlush(); //Đảm bảo tất cả các log đang chờ được đẩy đi trước khi ứng dụng tắt
}
