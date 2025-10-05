using Microsoft.AspNetCore.Mvc;
using Project.FeatureToggle.Core.Services.Interfaces;
using Project.FeatureToggle.Domain.Requests;

namespace Project.FeatureToggle.Api.Controllers;

[ApiController, Route("api/feature/manager")]
public class FeatureManagerController(IFeatureManagerService featureToggleService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFeatureToggle([FromRoute] Guid id)
        => Ok(await featureToggleService.GetFeature(id));

    [HttpGet]
    public async Task<IActionResult> GetFeatureToggle([FromQuery] FeatureQueryRequest request)
        => Ok(await featureToggleService.GetFeaturesPaged(request));

    [HttpPost]
    public async Task<IActionResult> CreateFeatureToggle([FromBody] FeatureRequest request)
        => Ok(await featureToggleService.CreateFeature(request));

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeatureToggle([FromRoute] Guid id, [FromBody] FeatureRequest request)
        => Ok(await featureToggleService.UpdateFeature(id, request));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeatureToggle([FromRoute] Guid id)
        => Ok(await featureToggleService.DeleteFeature(id));
}