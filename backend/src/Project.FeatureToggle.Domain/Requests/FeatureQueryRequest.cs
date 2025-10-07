using Project.FeatureToggle.Domain.Constants;

namespace Project.FeatureToggle.Domain.Requests;

/// <summary>
/// Representa os parâmetros de consulta para a listagem de features.
/// </summary>
/// <remarks>
/// Esta requisição permite filtrar, pesquisar e paginar os resultados de features.
/// 
/// **Regras:**
/// - <c>Filter</c>: Define o tipo de features a serem retornadas.
/// - <c>Search</c>: Texto de pesquisa aplicado ao nome ou descrição das features.
/// - <c>Page</c>: Número da página (mínimo: 1).
/// - <c>Quantity</c>: Quantidade de itens por página (padrão: 6, máximo: 6).
/// </remarks>
public record FeatureQueryRequest
{
    private int _page = 1;
    private int _quantity = 6;

    /// <summary>
    /// Filtro aplicado na consulta das features.
    /// </summary>
    /// <remarks>
    /// Valores possíveis (de <see cref="FeatureFilter"/>):
    /// - <c>all</c>: Retorna todas as features (padrão).
    /// - <c>active</c>: Retorna apenas as features ativas.
    /// - <c>inactive</c>: Retorna apenas as features inativas.
    /// </remarks>
    public string Filter { get; set; } = FeatureFilter.ALL;

    /// <summary>
    /// Texto utilizado para buscar features pelo nome ou descrição.
    /// </summary>
    /// <remarks>
    /// Se vazio ou nulo, nenhuma pesquisa textual será aplicada.
    /// </remarks>
    public string Search { get; set; }

    /// <summary>
    /// Número da página para a paginação.
    /// </summary>
    /// <remarks>
    /// O valor mínimo aceito é 1.  
    /// Se informado um valor menor que 1, a página padrão será mantida.
    /// </remarks>
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? _page : value;
    }

    /// <summary>
    /// Quantidade de itens retornados por página.
    /// </summary>
    /// <remarks>
    /// O valor padrão é 6.  
    /// Valores menores que 1 ou maiores que 6 são ignorados, mantendo o valor atual.
    /// </remarks>
    public int Quantity
    {
        get => _quantity;
        set => _quantity = value < 1 || value > 6 ? _quantity : value;
    }
}
