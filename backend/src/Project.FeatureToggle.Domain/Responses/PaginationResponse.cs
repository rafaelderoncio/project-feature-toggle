namespace Project.FeatureToggle.Domain.Responses;

/// <summary>
/// Representa uma resposta paginada genérica para listagens de dados na API.
/// </summary>
/// <typeparam name="T">Tipo dos itens retornados na página.</typeparam>
/// <remarks>
/// Este record é utilizado para encapsular dados paginados, incluindo informações
/// de navegação entre páginas.
///
/// Exemplo de JSON retornado:
/// ```json
/// {
///   "items": [
///     { "id": "1", "name": "FeatureA", "active": true },
///     { "id": "2", "name": "FeatureB", "active": false }
///   ],
///   "totalRecords": 42,
///   "totalPages": 7,
///   "page": 1,
///   "quantity": 6,
///   "previousPage": null,
///   "nextPage": 2
/// }
/// ```
/// </remarks>
public record PaginationResponse<T> where T : class
{
    /// <summary>
    /// Lista de itens retornados na página atual.
    /// </summary>
    public IReadOnlyList<T> Items { get; init; } = [];

    /// <summary>
    /// Total de registros disponíveis na consulta.
    /// </summary>
    public long TotalRecords { get; init; }

    /// <summary>
    /// Total de páginas disponíveis com base na quantidade por página.
    /// </summary>
    public int TotalPages { get; init; }

    /// <summary>
    /// Número da página atual.
    /// </summary>
    public int Page { get; init; }

    /// <summary>
    /// Quantidade de itens retornados por página.
    /// </summary>
    public int Quantity { get; init; }

    /// <summary>
    /// Número da página anterior, se houver.
    /// </summary>
    /// <remarks>
    /// Retorna <c>null</c> se a página atual for a primeira.
    /// </remarks>
    public int? PreviousPage { get; init; }

    /// <summary>
    /// Número da próxima página, se houver.
    /// </summary>
    /// <remarks>
    /// Retorna <c>null</c> se a página atual for a última.
    /// </remarks>
    public int? NextPage { get; init; }
}
