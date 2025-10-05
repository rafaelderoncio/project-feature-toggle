using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.FeatureToggle.Core.Services.Interfaces;

namespace Project.FeatureToggle.Api.Controllers;

[ApiController, Route("api/feature/dashboard")]
public class FeatureDashboard(IFeatureDashboardService featureDashboardService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFeatureToggle()
        => Ok(await featureDashboardService.GetDashboard());
}