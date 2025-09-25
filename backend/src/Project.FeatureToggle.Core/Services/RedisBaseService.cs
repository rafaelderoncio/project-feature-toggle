using Project.FeatureToggle.Core.Configurations.Settings;
using StackExchange.Redis;

namespace Project.FeatureToggle.Core.Services;

public abstract class RedisBaseService(RedisSettings settings)
{
    public ConnectionMultiplexer GetConnection()
        => ConnectionMultiplexer.Connect(
            new ConfigurationOptions {
                EndPoints = { { settings.Endpoint, settings.Port } },
                User = settings.Username,
                Password = settings.Password
            }
        );

    public async Task<string> GetValue()
    {
        using var connection = GetConnection();
        return await GetConnection().GetDatabase().StringGetAsync("");
    }

    public async Task<string?> GetValue(string key)
    {
        using var connection = GetConnection();
        return await connection.GetDatabase().StringGetAsync(key);
    }

    public async Task<bool> RemoveValue(string key)
    {
        using var connection = GetConnection();
        return await connection.GetDatabase().KeyDeleteAsync(key);
    }


    public async Task<bool> SetValue(string key, string value, TimeSpan? expiry)
    {
        using var connection = GetConnection();
        return await connection.GetDatabase().StringSetAsync(key, value, expiry);
    }
}