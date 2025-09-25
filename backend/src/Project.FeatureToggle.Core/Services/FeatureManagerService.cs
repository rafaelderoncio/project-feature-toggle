using System.Text.Json;
using Microsoft.Extensions.Logging;
using Project.FeatureToggle.Core.Repositories.Interfaces;
using Project.FeatureToggle.Core.Services.Interfaces;
using Project.FeatureToggle.Domain.Requests;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Services;

public class FeatureManagerService(IFeatureRepository repository, ILogger<FeatureManagerService> logger) : IFeatureManagerService
{
    public async Task<FeatureResponse> CreateFeature(FeatureRequest request)
    {
        try
        {
            logger.LogInformation("Starts CreateFeature with request: {0}", JsonSerializer.Serialize(request));

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
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on process CreateFeature");
            throw;
        }
        finally
        {
            logger.LogInformation("Fisnish CreateFeature");
        }
    }

    public async Task<FeatureResponse> DeleteFeature(Guid id)
    {
        try
        {
            logger.LogInformation("Starts DeleteFeature with id: {0}", id);

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
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on process DeleteFeature");
            throw;
        }
        finally
        {
            logger.LogInformation("Fisnish DeleteFeature");
        }
    }

    public async Task<FeatureResponse> GetFeature(Guid id)
    {
        try
        {
            logger.LogInformation("Starts GetFeature with id: {0}", id);

            var feature = await repository.GetFeature(id);

            return new FeatureResponse()
            {
                Id = feature.Id.ToString(),
                Name = feature.Name,
                Description = feature.Description,
                Feature = feature.Feature,
                Tags = feature.Tags,
                Active = feature.Active
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on process DeleteFeature");
            throw;
        }
        finally
        {
            logger.LogInformation("Fisnish DeleteFeature");
        }
    }

    public async Task<FeatureResponse[]> GetFeatures()
    {
        try
        {
            logger.LogInformation("Starts GetFeatures");

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
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on process GetFeatures");
            throw;
        }
        finally
        {
            logger.LogInformation("Fisnish GetFeatures");
        }
    }

    public async Task<PaginationResponse<FeatureResponse>> GetPagedFeatures(PaginationRequest request)
    {
        try
        {
            logger.LogInformation("Starts GetPagedFeatures with request: {0}", JsonSerializer.Serialize(request));

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
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on process DeleteFeature");
            throw;
        }
        finally
        {
            logger.LogInformation("Fisnish DeleteFeature");
        }
    }

    public async Task<FeatureResponse> UpdateFeature(Guid id, FeatureRequest request)
    {
        try
        {
            logger.LogInformation("Starts UpdateFeature with id: {0} and request: {1}", id, JsonSerializer.Serialize(request));

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
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on process UpdateFeature");
            throw;
        }
        finally
        {
            logger.LogInformation("Fisnish UpdateFeature");
        }
    }
}
