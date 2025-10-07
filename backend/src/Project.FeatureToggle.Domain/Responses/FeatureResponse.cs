namespace Project.FeatureToggle.Domain.Responses;

/// <summary>
/// Representa os dados de uma feature retornados pela API.
/// </summary>
/// <remarks>
/// Este record é utilizado em endpoints de consulta (GET) para retornar informações detalhadas
/// sobre uma feature específica ou lista de features.
///
/// Exemplo de JSON retornado:
/// ```json
/// {
///   "id": "c1d8b9f7-4e52-41e1-b0ad-88ef56a6b742",
///   "feature": "NewCheckout",
///   "name": "Novo Checkout",
///   "description": "Nova experiência de checkout para clientes",
///   "tags": ["checkout", "experiment", "release2025"],
///   "active": true
/// }
/// ```
/// </remarks>
public record FeatureResponse
{
    /// <summary>
    /// Identificador único da feature.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Código ou chave única da feature.
    /// </summary>
    /// <remarks>
    /// Exemplo: "NewCheckout", usado internamente para referência do toggle.
    /// </remarks>
    public string Feature { get; set; }

    /// <summary>
    /// Nome amigável da feature.
    /// </summary>
    /// <remarks>
    /// Exemplo: "Novo Checkout", utilizado para exibição ao usuário.
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Descrição detalhada da feature.
    /// </summary>
    /// <remarks>
    /// Explica o objetivo ou funcionalidade da feature.
    /// Exemplo: "Nova experiência de checkout para clientes".
    /// </remarks>
    public string Description { get; set; }

    /// <summary>
    /// Lista de tags associadas à feature.
    /// </summary>
    /// <remarks>
    /// Ajuda a categorizar e filtrar features.  
    /// Exemplo: ["checkout", "experiment", "release2025"]
    /// </remarks>
    public string[] Tags { get; set; } = [];

    /// <summary>
    /// Indica se a feature está ativa ou desativada.
    /// </summary>
    public bool Active { get; set; }
}
