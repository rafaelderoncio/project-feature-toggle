using Microsoft.Extensions.Options;
using Project.FeatureToggle.Core.Configurations.Settings;
using Project.FeatureToggle.Core.Services.Interfaces;

namespace Project.FeatureToggle.Core.Services;

public sealed class CacheService(IOptions<RedisSettings> options) : RedisBaseService(options.Value), ICacheService
{
    public async Task<string> GetValueAsync(string key)
        => await base.GetValue(key) ?? string.Empty;

    public async Task RemoveValueAsync(string key)
        => await base.RemoveValue(key);

    public async Task SetValueAsync(string key, string value)
        => await base.SetValue(key, value, TimeSpan.FromMinutes(options.Value.ExpiresInMinutes));
}
