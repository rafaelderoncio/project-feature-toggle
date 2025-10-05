using Project.FeatureToggle.Domain.Requests;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Services.Interfaces;

public interface IFeatureManagerService
{
    public Task<FeatureResponse[]> GetFeatures();
    public Task<PaginationResponse<FeatureResponse>> GetFeaturesPaged(PaginationRequest request);
    public Task<FeatureResponse> GetFeature(Guid id);
    public Task<FeatureResponse> CreateFeature(FeatureRequest request);
    public Task<FeatureResponse> UpdateFeature(Guid id, FeatureRequest request);
    public Task<FeatureResponse> DeleteFeature(Guid id);
}
