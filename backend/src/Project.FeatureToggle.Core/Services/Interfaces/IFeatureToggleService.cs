using System;
using Project.FeatureToggle.Domain.Requests;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Services.Interfaces;

public interface IFeatureToggleService
{
    public Task<FeatureToggleResponse[]> GetFeatureToggle();
    public Task<FeatureToggleResponse> GetFeatureToggle(Guid id);
    public Task<FeatureToggleResponse> CreateFeatureToggle(FeatureToggleRequest request);
    public Task<FeatureToggleResponse> UpdateFeatureToggle(Guid id, FeatureToggleRequest request);
    public Task<FeatureToggleResponse> DeleteFeatureToggle(Guid id);
}
