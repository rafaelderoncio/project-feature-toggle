using System.Linq.Expressions;
using Project.FeatureToggle.Core.Arguments;
using Project.FeatureToggle.Core.Models;

namespace Project.FeatureToggle.Core.Repositories.Interfaces;

/// <summary>
/// Interface para operações de acesso a dados de features.
/// </summary>
/// <remarks>
/// Define os métodos para criar, atualizar, consultar e deletar features diretamente no repositório,
/// bem como consultas agregadas, como total de features e status.
/// </remarks>
public interface IFeatureRepository
{
    /// <summary>
    /// Recupera uma feature pelo seu identificador único.
    /// </summary>
    /// <param name="id">Identificador da feature.</param>
    /// <returns>A feature correspondente ou <c>null</c> se não encontrada.</returns>
    Task<FeatureModel> GetFeature(Guid id);

    /// <summary>
    /// Recupera uma feature pelo seu nome ou chave única.
    /// </summary>
    /// <param name="feature">Nome ou chave da feature.</param>
    /// <returns>A feature correspondente ou <c>null</c> se não encontrada.</returns>
    Task<FeatureModel> GetFeature(string feature);

    /// <summary>
    /// Recupera todas as features cadastradas.
    /// </summary>
    /// <returns>Array contendo todas as features.</returns>
    Task<FeatureModel[]> GetFeatures();

    /// <summary>
    /// Recupera features filtradas e paginadas de acordo com os argumentos fornecidos.
    /// </summary>
    /// <param name="argument">Argumentos de filtro e paginação.</param>
    /// <returns>Array contendo as features correspondentes.</returns>
    Task<FeatureModel[]> GetFeatures(FeatureArgument argument);

    /// <summary>
    /// Obtém o total de features que atendem aos filtros fornecidos.
    /// </summary>
    /// <param name="argument">Argumentos de filtro.</param>
    /// <returns>Total de features correspondentes.</returns>
    Task<long> GetTotalFeatures(FeatureArgument argument);

    /// <summary>
    /// Salva uma nova feature no repositório.
    /// </summary>
    /// <param name="model">Modelo da feature a ser salva.</param>
    /// <returns>A feature salva com os valores persistidos.</returns>
    Task<FeatureModel> SaveFeature(FeatureModel model);

    /// <summary>
    /// Atualiza uma feature existente no repositório.
    /// </summary>
    /// <param name="model">Modelo da feature com os dados atualizados.</param>
    /// <returns>A feature atualizada.</returns>
    Task<FeatureModel> UpdateFeature(FeatureModel model);

    /// <summary>
    /// Atualiza um campo específico de uma feature pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador da feature a ser atualizada.</param>
    /// <param name="field">Expressão indicando o campo a ser atualizado.</param>
    /// <param name="value">Novo valor para o campo.</param>
    /// <returns>A feature atualizada.</returns>
    Task<FeatureModel> UpdateFeature(Guid id, Expression<Func<FeatureModel, object>> field, object value);

    /// <summary>
    /// Deleta uma feature pelo seu identificador.
    /// </summary>
    /// <param name="id">Identificador da feature a ser deletada.</param>
    /// <returns>A feature deletada.</returns>
    Task<FeatureModel> DeleteFeature(Guid id);

    /// <summary>
    /// Retorna o status agregado das features.
    /// </summary>
    /// <returns>
    /// Uma tupla contendo:
    /// <list type="bullet">
    /// <item><description>Total de features ativas</description></item>
    /// <item><description>Total de features inativas</description></item>
    /// <item><description>Total de features cadastradas</description></item>
    /// </list>
    /// </returns>
    Task<(int totalActives, int totalInactives, int totalFeatures)> GetFeatureStatus();
}
