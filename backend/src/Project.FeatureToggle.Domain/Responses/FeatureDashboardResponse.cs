namespace Project.FeatureToggle.Domain.Responses;

/// <summary>
/// Representa a resposta do dashboard de feature toggles.
/// </summary>
/// <remarks>
/// Este record é retornado pelo endpoint do dashboard e fornece informações agregadas
/// sobre o estado das features da aplicação.
///
/// Exemplo de JSON retornado:
/// ```json
/// {
///   "totalActives": 18,
///   "totalInactives": 7,
///   "totalFeatures": 25
/// }
/// ```
/// </remarks>
public record FeatureDashboardResponse
{
    /// <summary>
    /// Quantidade total de features ativas.
    /// </summary>
    public int TotalActives { get; set; }

    /// <summary>
    /// Quantidade total de features inativas.
    /// </summary>
    public int TotalInactives { get; set; }

    /// <summary>
    /// Quantidade total de features cadastradas.
    /// </summary>
    public int TotalFeatures { get; set; }
}
