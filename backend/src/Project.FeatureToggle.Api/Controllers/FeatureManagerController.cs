using Microsoft.AspNetCore.Mvc;
using Project.FeatureToggle.Core.Services.Interfaces;
using Project.FeatureToggle.Domain.Requests;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Api.Controllers;

/// <summary>
/// Controlador responsável por gerenciar o ciclo de vida dos feature toggles.
/// </summary>
[ApiController]
[Route("api/feature/manager")]
public class FeatureManagerController : ControllerBase
{
    private readonly IFeatureManagerService _service;

    public FeatureManagerController(IFeatureManagerService service)
    {
        _service = service;
    }

    /// <summary>
    /// Obtém uma feature específica pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador único da feature.</param>
    /// <returns>Retorna os detalhes da feature.</returns>
    /// <response code="200">Feature encontrada e retornada com sucesso.</response>
    /// <response code="404">Feature não encontrada.</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFeature([FromRoute] Guid id)
        => Ok(await _service.GetFeature(id));

    /// <summary>
    /// Obtém uma lista paginada de features com base nos filtros informados.
    /// </summary>
    /// <param name="request">Filtros e parâmetros de paginação.</param>
    /// <returns>Retorna a lista paginada de features.</returns>
    /// <response code="200">Lista de features retornada com sucesso.</response>
    [HttpGet]
    public async Task<IActionResult> GetFeatures([FromQuery] FeatureQueryRequest request)
        => Ok(await _service.GetFeatures(request));

    /// <summary>
    /// Cria uma nova feature.
    /// </summary>
    /// <param name="request">Dados necessários para criação da feature.</param>
    /// <returns>Retorna a feature criada.</returns>
    /// <response code="201">Feature criada com sucesso.</response>
    /// <response code="400">Dados inválidos para criação.</response>
    [HttpPost]
    public async Task<IActionResult> CreateFeature([FromBody] FeatureRequest request)
        => Ok(await _service.CreateFeature(request));

    /// <summary>
    /// Atualiza os dados de uma feature existente.
    /// </summary>
    /// <param name="id">Identificador da feature a ser atualizada.</param>
    /// <param name="request">Dados para atualização da feature.</param>
    /// <returns>Retorna a feature atualizada.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFeature([FromRoute] Guid id, [FromBody] FeatureRequest request)
        => Ok(await _service.UpdateFeature(id, request));

    /// <summary>
    /// Exclui uma feature pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador da feature a ser removida.</param>
    /// <returns>Retorna o resultado da exclusão.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(type: typeof(FeatureResponse), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(type: typeof(ExceptionResponse), statusCode: StatusCodes.Status404NotFound)]
    [ProducesResponseType(type: typeof(ExceptionResponse), statusCode:StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteFeature([FromRoute] Guid id)
        => Ok(await _service.DeleteFeature(id));
}
