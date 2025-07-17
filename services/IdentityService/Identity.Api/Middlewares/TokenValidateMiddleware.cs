using BuildingBlocks.Interfaces;
using Identity.Application.Interfaces;
using Identity.Application.Services;

namespace Identity.Api.Middlewares;

public class TokenValidateMiddleware(ILogger<TokenValidateMiddleware> logger, RequestDelegate next, IRedisService redisService, IJwtService jwtService)
{
    public async Task Invoke(HttpContext context)
    {
        logger.LogInformation("TokenValidateMiddleware is processing request: {Path}", context.Request.Path);
        var path = context.Request.Path.Value?.ToLowerInvariant();

        // Bỏ qua middleware cho các endpoint không cần xác thực
        if (path != null &&
            (path.StartsWith("authen/login") ||
             path.StartsWith("authen/refreshtoken") ||
             path.StartsWith("authen/googlelogin") ||
             path.StartsWith("authen/googlecallback") ||
             path.StartsWith("authen/register") ||
             path.StartsWith("scalar")))
        {
            await next(context);
            return;
        }

        string? token = null;

        // ✅ Ưu tiên lấy token từ Authorization Header
        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            token = authHeader.Substring("Bearer ".Length).Trim();
        }

        if (string.IsNullOrEmpty(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing token");
            return;
        }

        try
        {
            var userId = jwtService.GetUserIdFromToken(token);

            // Kiểm tra token lưu trong Redis
            var redisKey = $"accessToken:{userId}";
            var storedToken = await redisService.GetValue(redisKey);
            if (string.IsNullOrEmpty(storedToken) || storedToken != token)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid or expired token");
                return;
            }

            // Lưu userId vào context để sử dụng cho các middleware hoặc controller sau
            context.Items["UserId"] = userId;
            context.Items["AccessToken"] = token;
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: " + ex.Message);
            return;
        }

        // Nếu token hợp lệ, tiếp tục xử lý request
        await next(context);
    }
}