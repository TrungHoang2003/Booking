// Load environment-specific .env file
using DotNetEnv;
using Host.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Settings.Configuration;

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

// Get connection string from configuration
var postgresConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

// Add Infrastructure layer with PostgreSQL configuration
builder.Services.AddInfrastructure(postgresConnectionString);

// Add Application layer
//builder.Services.AddApplication();

// Add Controllers
builder.Services.AddControllers();

// Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration) // Đọc cấu hình từ appsettings.json
    .ReadFrom.Services(services) // Cho phép DI cho các enricher/sinks nếu cần
);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseSerilogRequestLogging();
app.MapControllers();

app.UseHttpsRedirection();

try
{
    Log.Information("Starting Host Service...");
    Log.Information("Using connection string: {ConnectionString}", postgresConnectionString);
    Log.Information("Environment: {Environment}", environment);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host Service start-up failed");
}
finally
{
    Log.CloseAndFlush(); //Đảm bảo tất cả các log đang chờ được đẩy đi trước khi ứng dụng tắt
}
