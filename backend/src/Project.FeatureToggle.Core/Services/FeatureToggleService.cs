using Project.FeatureToggle.Core.Repositories.Interfaces;
using Project.FeatureToggle.Core.Services.Interfaces;
using Project.FeatureToggle.Domain.Requests;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Services;

public class FeatureToggleService(IFeatureToggleRepository featureToggleRepository) : IFeatureToggleService
{


    public async Task<FeatureToggleResponse> CreateFeatureToggle(FeatureToggleRequest request)
    {
        var feature = await featureToggleRepository.SaveFeatureToggle(new()
        {
            Name = request.Name,
            Description = request.Description,
            Feature = request.Name.ToLower().Replace(" ", "-"),
            Tags = [.. request.Tags.Select(tag => "#" + tag.ToLower().Replace(" ", "-")).Distinct()],
            Active = request.Active,
        });

        return new()
        {
            Id = feature.Id.ToString(),
            Name = feature.Name,
            Description = feature.Description,
            Feature = feature.Feature,
            Tags = feature.Tags,
            Active = feature.Active
        };
    }

    public async Task<FeatureToggleResponse> DeleteFeatureToggle(Guid id)
    {
        var feature = await featureToggleRepository.DeleteFeatureToggle(id);
        return feature is null ? new() : new()
        {
            Id = feature.Id.ToString(),
            Name = feature.Name,
            Description = feature.Description,
            Feature = feature.Feature,
            Tags = feature.Tags,
            Active = feature.Active
        };
    }

    public async Task<FeatureToggleResponse> GetFeatureToggle(Guid id)
    {
        var feature = await featureToggleRepository.GetFeatureToggle(id);
        return feature is null ? new() : new()
        {
            Id = feature.Id.ToString(),
            Name = feature.Name,
            Description = feature.Description,
            Feature = feature.Feature,
            Tags = feature.Tags,
            Active = feature.Active
        };
    }

    public async Task<FeatureToggleResponse[]> GetFeatureToggle()
    {
        var features = await featureToggleRepository.GetFeatureToggle();
        return [.. features.Select(feature => new FeatureToggleResponse
        {
            Id = feature.Id.ToString(),
            Name = feature.Name,
            Description = feature.Description,
            Feature = feature.Feature,
            Tags = feature.Tags,
            Active = feature.Active
        })];
    }

    public async Task<FeatureToggleResponse> UpdateFeatureToggle(Guid id, FeatureToggleRequest request)
    {
        var feature = await featureToggleRepository.GetFeatureToggle(id);

        feature.Name = string.IsNullOrEmpty(request.Name) ? feature.Name : request.Name;
        feature.Description = string.IsNullOrEmpty(request.Description) ? feature.Description : request.Description;
        feature.Tags = request.Tags;
        feature.Active = request.Active;

        var featureUpdated = await featureToggleRepository.UpdateFeatureToggle(feature);

        return feature is null ? new() : new()
        {
            Id = featureUpdated.Id.ToString(),
            Name = featureUpdated.Name,
            Description = featureUpdated.Description,
            Feature = featureUpdated.Feature,
            Tags = featureUpdated.Tags,
            Active = featureUpdated.Active
        };
    }
}
