using Project.FeatureToggle.Domain.Requests;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Services.Interfaces;

/// <summary>
/// Interface para gerenciamento completo de feature toggles.
/// </summary>
/// <remarks>
/// Esta interface define métodos para criar, atualizar, consultar, listar (com paginação) 
/// e deletar features da aplicação.
/// </remarks>
public interface IFeatureManagerService
{
    /// <summary>
    /// Recupera todas as features cadastradas sem paginação.
    /// </summary>
    /// <returns>
    /// Um array de <see cref="FeatureResponse"/> contendo todas as features.
    /// </returns>
    Task<FeatureResponse[]> GetFeatures();

    /// <summary>
    /// Recupera features paginadas de acordo com os filtros fornecidos.
    /// </summary>
    /// <param name="request">Parâmetros de consulta e paginação da lista de features.</param>
    /// <returns>
    /// Um <see cref="PaginationResponse{FeatureResponse}"/> contendo a página de features, total de registros,
    /// total de páginas e informações de navegação entre páginas.
    /// </returns>
    Task<PaginationResponse<FeatureResponse>> GetFeatures(FeatureQueryRequest request);

    /// <summary>
    /// Recupera os detalhes de uma feature específica pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador único da feature.</param>
    /// <returns>
    /// Um <see cref="FeatureResponse"/> com os detalhes da feature.
    /// </returns>
    Task<FeatureResponse> GetFeature(Guid id);

    /// <summary>
    /// Cria uma nova feature na aplicação.
    /// </summary>
    /// <param name="request">Dados da feature a ser criada.</param>
    /// <returns>
    /// Um <see cref="FeatureResponse"/> representando a feature criada.
    /// </returns>
    Task<FeatureResponse> CreateFeature(FeatureRequest request);

    /// <summary>
    /// Atualiza uma feature existente pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador único da feature a ser atualizada.</param>
    /// <param name="request">Novos dados da feature.</param>
    /// <returns>
    /// Um <see cref="FeatureResponse"/> representando a feature atualizada.
    /// </returns>
    Task<FeatureResponse> UpdateFeature(Guid id, FeatureRequest request);

    /// <summary>
    /// Deleta uma feature pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador único da feature a ser removida.</param>
    /// <returns>
    /// Um <see cref="FeatureResponse"/> representando a feature deletada.
    /// </returns>
    Task<FeatureResponse> DeleteFeature(Guid id);
}
