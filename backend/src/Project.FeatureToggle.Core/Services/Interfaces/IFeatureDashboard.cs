using System;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Services.Interfaces;

public interface IFeatureDashboardService
{
    Task<FeatureDashboardResponse> GetDashboard();
}
