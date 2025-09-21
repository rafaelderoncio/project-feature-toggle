using Project.FeatureToggle.Core.Models;
using Project.FeatureToggle.Core.Repositories.Interfaces;

namespace Project.FeatureToggle.Core.Repositories;

public sealed class FeatureToggleRepository : IFeatureToggleRepository
{
    public FeatureToggleRepository()
    {
        _features = [.. Enumerable.Range(1, 100)
            .Select(i => new FeatureToggleModel
            {
                Id = Guid.NewGuid(),
                Feature = $"feature-{i}",
                Name = $"Feature {i}",
                Description = $"Descrição da feature {i}",
                Tags = [.. Enumerable.Range(1, 5).Select(t => $"#{t:D2}-feature-{i}")],
                Active = i % 2 == 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            })];
    }

    private FeatureToggleModel[] _features = [];

    public async Task<FeatureToggleModel> GetFeatureToggle(Guid id)
    {
        return await Task.FromResult(_features.FirstOrDefault(x => x.Id == id));
    }

    public async Task<FeatureToggleModel> GetFeatureToggle(string feature)
    {
        return await Task.FromResult(_features.FirstOrDefault(x => x.Feature == feature));
    }

    public async Task<FeatureToggleModel[]> GetFeatureToggle()
    {
        return await Task.FromResult(this._features);
    }

    public async Task<FeatureToggleModel> SaveFeatureToggle(FeatureToggleModel model)
    {
        if (!_features.Any(x => x.Feature == model.Feature))
        {
            model.Id = Guid.NewGuid();
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;
            _features.Append(model);
            return await Task.FromResult(model);
        }

        return null;
    }

    public async Task<FeatureToggleModel> UpdateFeatureToggle(FeatureToggleModel model)
    {
        if (_features.Any(x => x.Id == model.Id))
        {
            model.UpdatedAt = DateTime.UtcNow;
            _features.Append(model);
            return await Task.FromResult(model);
        }

        return null;
    }

    public async Task<FeatureToggleModel> DeleteFeatureToggle(Guid id)
    {
        var feature = _features.FirstOrDefault(x => x.Id == id);
        if (feature is not null)
        {
            _features = [.. _features.ToList().Where(x => x.Id != id)];
            return await Task.FromResult(feature);
        }

        return null;
    }
}
