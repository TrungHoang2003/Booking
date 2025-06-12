using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;
using ApiGateway.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// 1. Thêm dịch vụ YARP vào Dependency Injection container
// YARP đọc cấu hình từ file appsettings.json

var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6379";
builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
    ConnectionMultiplexer.Connect(redisConnectionString));

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseMiddleware<GatewayAuthMiddleware>();

// 2. Thêm middleware YARP vào pipeline yêu cầu
app.MapReverseProxy();
// YARP sẽ xử lý các yêu cầu được định tuyến.

app.Run();
