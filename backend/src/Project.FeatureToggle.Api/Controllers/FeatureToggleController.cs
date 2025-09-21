using Microsoft.AspNetCore.Mvc;
using Project.FeatureToggle.Core.Services.Interfaces;
using Project.FeatureToggle.Domain.Requests;

namespace Project.FeatureToggle.Api.Controllers;

[ApiController, Route("api/feature-toggle")]
public class FeatureToggleController(IFeatureToggleService featureToggleService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFeatureToggle([FromRoute] Guid id)
        => Ok(await featureToggleService.GetFeatureToggle(id));

    [HttpGet]
    public async Task<IActionResult> GetFeatureToggle([FromQuery] FeatureToggleQueryRequest query)
        => Ok(await featureToggleService.GetFeatureToggle());

    [HttpPost]
    public async Task<IActionResult> CreateFeatureToggle([FromBody] FeatureToggleRequest request)
        => Ok(await featureToggleService.CreateFeatureToggle(request));

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeatureToggle([FromRoute] Guid id, [FromBody] FeatureToggleRequest request)
        => Ok(await featureToggleService.UpdateFeatureToggle(id, request));

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFeatureToggle([FromRoute] Guid id)
        => Ok(await featureToggleService.DeleteFeatureToggle(id));
}