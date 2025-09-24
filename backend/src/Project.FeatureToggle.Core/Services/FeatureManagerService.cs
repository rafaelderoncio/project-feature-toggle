using System.Text.Json;
using Project.FeatureToggle.Core.Repositories.Interfaces;
using Project.FeatureToggle.Core.Services.Interfaces;
using Project.FeatureToggle.Domain.Requests;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Services;

public class FeatureManagerService(IFeatureRepository repository, ICacheService cache) : IFeatureManagerService
{
    public async Task<FeatureResponse> CreateFeature(FeatureRequest request)
    {
        var feature = await repository.SaveFeature(new()
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

    public async Task<FeatureResponse> DeleteFeature(Guid id)
    {
        var feature = await repository.DeleteFeature(id);
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

    public async Task<FeatureResponse> GetFeature(Guid id)
    {
        var featureCached = await cache.GetValueAsync(id.ToString());
        if (!string.IsNullOrEmpty(featureCached))
        {
            return JsonSerializer.Deserialize<FeatureResponse>(featureCached);
        }

        var feature = await repository.GetFeature(id);
        if (feature is not null)
        {
            var response = new FeatureResponse()
            {
                Id = feature.Id.ToString(),
                Name = feature.Name,
                Description = feature.Description,
                Feature = feature.Feature,
                Tags = feature.Tags,
                Active = feature.Active
            };

            await cache.SetValueAsync(
                key: response.Id,
                value: JsonSerializer.Serialize(response)
            );

            return response;
        }

        return new();
    }

    public async Task<FeatureResponse[]> GetFeatures()
    {
        var features = await repository.GetFeatures();
        return [.. features.Select(feature => new FeatureResponse
        {
            Id = feature.Id.ToString(),
            Name = feature.Name,
            Description = feature.Description,
            Feature = feature.Feature,
            Tags = feature.Tags,
            Active = feature.Active
        })];
    }

    public async Task<PaginationResponse<FeatureResponse>> GetPagedFeatures(PaginationRequest request)
    {
        var features = await repository.GetFeatures(
            onlyActive: request.OnlyActive,
            quantity: request.Quantity,
            page: request.Page);

        var totalRecords = await repository.GetTotalFeatures(request.OnlyActive);

        var totalPages = (int)Math.Ceiling(totalRecords / (double)request.Quantity);

        var responses = features.Select(feature => new FeatureResponse
            {
                Id = feature.Id.ToString(),
                Name = feature.Name,
                Description = feature.Description,
                Feature = feature.Feature,
                Tags = feature.Tags,
                Active = feature.Active
            }
        );

        return new PaginationResponse<FeatureResponse>
        {
            Items = [.. responses],
            TotalRecords = totalRecords,
            TotalPages = totalPages,
            Page = request.Page,
            Quantity = request.Quantity,
            PreviousPage = request.Page > 1 ? request.Page - 1 : null,
            NextPage = request.Page < totalPages ? request.Page + 1 : null
        };
    }

    public async Task<FeatureResponse> UpdateFeature(Guid id, FeatureRequest request)
    {
        var feature = await repository.GetFeature(id);

        feature.Name = string.IsNullOrEmpty(request.Name) ? feature.Name : request.Name;
        feature.Description = string.IsNullOrEmpty(request.Description) ? feature.Description : request.Description;
        feature.Tags = request.Tags;
        feature.Active = request.Active;

        var featureUpdated = await repository.UpdateFeature(feature);

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
