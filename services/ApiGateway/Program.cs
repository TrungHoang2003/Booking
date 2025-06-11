var builder = WebApplication.CreateBuilder(args);

// 1. Thêm dịch vụ YARP vào Dependency Injection container
// YARP đọc cấu hình từ file appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

// 2. Thêm middleware YARP vào pipeline yêu cầu
app.MapReverseProxy();
// YARP sẽ xử lý các yêu cầu được định tuyến.

app.Run();
