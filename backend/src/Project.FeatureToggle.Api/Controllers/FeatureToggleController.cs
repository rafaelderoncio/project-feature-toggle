using Microsoft.AspNetCore.Mvc;
using Project.FeatureToggle.Core.Services.Interfaces;

namespace Project.FeatureToggle.Api.Controllers;

[ApiController, Route("api/feature/toggle")]
public class FeatureToggleController(IFeatureToggleService service) : ControllerBase
{
    [HttpGet("{name}")]
    public async Task<IActionResult> GetToggle([FromRoute] string name)
        => Ok(await service.GetToggle(name));

    [HttpPut("{name}")]
    public async Task<IActionResult> PutToggle([FromRoute] string name)
        => Ok(await service.PutToggle(name));
}