using Microsoft.Extensions.Logging;
using Project.FeatureToggle.Core.Repositories.Interfaces;
using Project.FeatureToggle.Core.Services.Interfaces;

namespace Project.FeatureToggle.Core.Services;

public sealed class FeatureToggleService(
    IFeatureRepository repository,
    ICacheService cache,
    ILogger<IFeatureToggleService> logger) : IFeatureToggleService
{
    public async Task<bool> GetToggle(string feature)
    {
        logger.LogInformation("Starts GetToggle for feature {0}", feature);

        try
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
        catch (System.Exception ex)
        {
            logger.LogError("Error on GetToggle for feature {0}. {1}", feature, ex.Message);
            throw;
        }
        finally
        {
            logger.LogError("Finish GetToggle for feature {0}.", feature);
        }
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
