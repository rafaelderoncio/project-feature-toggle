using Microsoft.AspNetCore.Mvc;
using Project.FeatureToggle.Core.Services.Interfaces;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Api.Controllers;

/// <summary>
/// Controlador responsável por operações diretas de consulta e alteração de toggles.
/// </summary>
[ApiController]
[Route("api/feature/toggle")]
public class FeatureToggleController : ControllerBase
{
    private readonly IFeatureToggleService _service;

    public FeatureToggleController(IFeatureToggleService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtém o estado atual de um toggle específico.
    /// </summary>
    /// <param name="name">Nome do toggle a ser consultado.</param>
    /// <returns>Retorna o estado atual (ativo ou inativo) do toggle.</returns>
    [HttpGet("{name}")]
    [ProducesResponseType(type: typeof(bool), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ExceptionResponse), statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(type: typeof(ExceptionResponse), statusCode:StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetToggle([FromRoute] string name)
        => Ok(await _service.GetToggle(name));

    /// <summary>
    /// Altera (ativa ou desativa) um toggle específico.
    /// </summary>
    /// <param name="name">Nome do toggle a ser alterado.</param>
    /// <returns>Retorna o novo estado do toggle.</returns>
    [HttpPut("{name}")]
    [HttpGet("{name}")]
    [ProducesResponseType(type: typeof(bool), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ExceptionResponse), statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(type: typeof(ExceptionResponse), statusCode:StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutToggle([FromRoute] string name)
        => Ok(await _service.PutToggle(name));
}
