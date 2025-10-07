namespace Project.FeatureToggle.Domain.Requests;

/// <summary>
/// Representa os dados necessários para criar ou atualizar uma feature.
/// </summary>
/// <remarks>
/// Esta requisição é utilizada nos endpoints de criação (<c>POST</c>) e atualização (<c>PUT</c>) de features.
/// 
/// Exemplo de JSON:
/// ```json
/// {
///   "name": "NewCheckout",
///   "description": "Nova experiência de checkout para clientes",
///   "tags": [ "checkout", "experiment", "release2025" ],
///   "active": true
/// }
/// ```
/// </remarks>
public record FeatureRequest
{
    /// <summary>
    /// Nome único da feature.
    /// </summary>
    /// <remarks>
    /// Deve ser descritivo e curto.  
    /// Exemplo: <c>NewCheckout</c>
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Descrição detalhada da feature.
    /// </summary>
    /// <remarks>
    /// Útil para fornecer contexto sobre o objetivo da feature.  
    /// Exemplo: <c>Nova experiência de checkout para aumentar a conversão</c>
    /// </remarks>
    public string Description { get; set; }

    /// <summary>
    /// Lista de tags associadas à feature.
    /// </summary>
    /// <remarks>
    /// Ajuda a organizar e agrupar features por categorias.  
    /// Exemplo: <c>["checkout", "experiment", "release2025"]</c>
    /// </remarks>
    public string[] Tags { get; set; } = [];

    /// <summary>
    /// Indica se a feature está ativa ou não.
    /// </summary>
    /// <remarks>
    /// Se <c>true</c>, a feature estará habilitada.  
    /// Se <c>false</c>, a feature permanecerá desabilitada.
    /// </remarks>
    public bool Active { get; set; }
}
