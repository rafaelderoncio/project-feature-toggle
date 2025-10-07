using Microsoft.AspNetCore.Mvc;
using Project.FeatureToggle.Core.Services.Interfaces;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Api.Controllers;

/// <summary>
/// Controlador responsável por fornecer os dados do painel (dashboard) das features.
/// </summary>
[ApiController]
[Route("api/feature/dashboard")]
public class FeatureDashboardController : ControllerBase
{
    private readonly IFeatureDashboardService _featureDashboardService;

    public FeatureDashboardController(IFeatureDashboardService featureDashboardService)
    {
        _featureDashboardService = featureDashboardService;
    }

    /// <summary>
    /// Obtém o painel com informações gerais sobre os feature toggles.
    /// </summary>
    /// <returns>Retorna os dados agregados do dashboard de features.</returns>
    [HttpGet]
    [ProducesResponseType(type: typeof(FeatureDashboardResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ExceptionResponse), statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(type: typeof(ExceptionResponse), statusCode:StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetFeatureDashboard()
        => Ok(await _featureDashboardService.GetDashboard());
}
