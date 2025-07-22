using StackExchange.Redis;
using System.IdentityModel.Tokens.Jwt;

namespace ApiGateway.Middlewares;

public class GatewayAuthMiddleware(RequestDelegate next, IConnectionMultiplexer redis, ILogger<GatewayAuthMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant();
        logger.LogInformation("GatewayAuthMiddleware is processing request: {Path}", path);

        // Skip authentication for login/register endpoints
        if (path != null && 
            (path.StartsWith("/identity/authen/login") ||
             path.StartsWith("/identity/authen/register") ||
             path.StartsWith("/identity/authen/refreshtoken")))
        {
            await next(context);
            return;
        }

        var authHeader = context.Request.Headers["Authorization"].ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Missing token");
            return;
        }

        var token = authHeader.Substring("Bearer ".Length).Trim();

        try
        {
            var userId = GetUserIdFromToken(token);
            
            // Check token in Redis
            var db = redis.GetDatabase();
            var redisKey = $"accessToken:{userId}";
            var storedToken = await db.StringGetAsync(redisKey);
            
            if (!storedToken.HasValue || storedToken != token)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid or expired token");
                return;            }

            // Add user info to headers for downstream services
            
            context.Request.Headers["X-User-Id"] = userId.ToString();
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Authentication failed");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: " + ex.Message);
        }
    }

    private int GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
            throw new Exception("Invalid token format");

        var jwtToken = handler.ReadJwtToken(token);
        var userIdStr = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            throw new Exception("Invalid user ID in token");

        return userId;
    }
}