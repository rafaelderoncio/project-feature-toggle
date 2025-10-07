using System;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Services.Interfaces;

/// <summary>
/// Interface para operações relacionadas ao dashboard de feature toggles.
/// </summary>
/// <remarks>
/// Fornece informações agregadas sobre o estado das features na aplicação,
/// como total de features ativas, inativas e o total geral.
/// </remarks>
public interface IFeatureDashboardService
{
    /// <summary>
    /// Recupera informações agregadas do dashboard de features.
    /// </summary>
    /// <returns>
    /// Um <see cref="FeatureDashboardResponse"/> contendo:
    /// <list type="bullet">
    /// <item><description>Total de features ativas</description></item>
    /// <item><description>Total de features inativas</description></item>
    /// <item><description>Total de features cadastradas</description></item>
    /// </list>
    /// </returns>
    Task<FeatureDashboardResponse> GetDashboard();
}
