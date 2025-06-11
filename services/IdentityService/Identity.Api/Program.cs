using BuildingBlocks.Middlewares;
using Identity.Api.Middlewares;
using Identity.Application;
using Identity.Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// Get connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Add Infrastructure layer with PostgreSQL configuration
builder.Services.AddInfrastructure(connectionString);

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
app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleWare>();
app.UseMiddleware<TokenValidateMiddleware>();
app.UseHttpsRedirection();
// Add Serilog request logging
app.UseSerilogRequestLogging();
app.MapControllers();

try
{
    Log.Information("Starting Identity Service...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Identity Service start-up failed");
}
finally
{
    Log.CloseAndFlush(); //Đảm bảo tất cả các log đang chờ được đẩy đi trước khi ứng dụng tắt
}

