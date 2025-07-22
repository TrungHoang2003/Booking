namespace BuildingBlocks.Interfaces;

public interface IRedisService
{
    Task SetValue(string key, string? value, TimeSpan? expiry = null);
    Task<string?> GetValue(string key);
    Task<bool> DeleteValue(string key);
}