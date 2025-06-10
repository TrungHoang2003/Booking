using StackExchange.Redis;

namespace Identity.Application.Commons;

public class RedisService(IConnectionMultiplexer redis)
{
    public async Task SetValue(string key, string? value, TimeSpan? expiry = null)
    {
        var db = redis.GetDatabase();
        await db.StringSetAsync(key, value, expiry);
    }

    public async Task<string> GetValue(string key)
    {
        var db = redis.GetDatabase();
        var value = await db.StringGetAsync(key);
        return value.HasValue ? value.ToString() : string.Empty; // Kiểm tra giá trị có tồn tại hay không
    }
    public async Task<bool> DeleteValue(string key)
    {
        var db = redis.GetDatabase();
        return await db.KeyDeleteAsync(key);
    }
}