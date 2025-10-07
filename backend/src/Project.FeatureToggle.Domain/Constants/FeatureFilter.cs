using System;

namespace Project.FeatureToggle.Domain.Constants;

/// <summary>
/// Define os filtros disponíveis para consulta de feature toggles.
/// </summary>
/// <remarks>
/// Esta estrutura é utilizada para padronizar os filtros aceitos pela API.
/// 
/// Valores possíveis:
/// - <c>all</c>: retorna todas as features.
/// - <c>active</c>: retorna apenas as features ativas.
/// - <c>inactive</c>: retorna apenas as features inativas.
/// </remarks>
public readonly struct FeatureFilter
{
    /// <summary>
    /// Filtro que retorna todas as features, independentemente do estado.
    /// </summary>
    public const string ALL = "all";

    /// <summary>
    /// Filtro que retorna apenas as features ativas.
    /// </summary>
    public const string ACTIVE = "active";

    /// <summary>
    /// Filtro que retorna apenas as features inativas.
    /// </summary>
    public const string INACTIVE = "inactive";
}
