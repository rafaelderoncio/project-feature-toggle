using Project.FeatureToggle.Core.Repositories.Interfaces;
using Project.FeatureToggle.Core.Services.Interfaces;

namespace Project.FeatureToggle.Core.Services;

public sealed class FeatureToggleService(IFeatureRepository repository, ICacheService cache) : IFeatureToggleService
{
    public async Task<bool> GetToggle(string feature)
    {
        var cached = await cache.GetValueAsync(feature);

        if (!string.IsNullOrEmpty(cached))
            return bool.Parse(cached);

        var model = await repository.GetFeature(feature);

        if (model is null)
            return false;

        await cache.SetValueAsync(feature, model.Active.ToString());

        return model.Active;
    }

    public async Task<bool> PutToggle(string feature)
    {
        var model = await repository.GetFeature(feature);

        if (model is null)
            return false;

        model.Active = !model.Active;

        await repository.UpdateFeature(model);

        await cache.SetValueAsync(feature, model.Active.ToString());

        return model.Active;
    }
}
