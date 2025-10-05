using System;
using Microsoft.Extensions.Logging;
using Project.FeatureToggle.Core.Exceptions;
using Project.FeatureToggle.Core.Repositories.Interfaces;
using Project.FeatureToggle.Core.Services.Interfaces;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Services;

public class FeatureDashboardService(IFeatureRepository repository, ILogger<IFeatureDashboardService> logger) : IFeatureDashboardService
{
    public async Task<FeatureDashboardResponse> GetDashboard()
    {
        try
        {
            (int total, int actives, int inactives) = await repository.GetFeatureStatus();
            return new()
            {
                TotalActives = actives,
                TotalInactives = inactives,
                TotalFeatures = total
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on process GetDashboard. {ex}", ex.Message);
            throw new BaseException(
                title: "Feature Dashboard",
                message: "Error on get feature dashboard.",
                type: Configurations.Enums.ErrorType.Fatal
            );
        }
    }
}
