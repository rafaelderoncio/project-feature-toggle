namespace Project.FeatureToggle.Core.Services.Interfaces;

public interface ICacheService
{
    Task SetValueAsync(string key, string value);

    Task<string> GetValueAsync(string key);

    Task RemoveValueAsync(string key);
}
