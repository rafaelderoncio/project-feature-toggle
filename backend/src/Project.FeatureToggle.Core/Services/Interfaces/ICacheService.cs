namespace Project.FeatureToggle.Core.Services.Interfaces;

/// <summary>
/// Interface que define operações básicas de cache para armazenamento e recuperação de valores.
/// </summary>
/// <remarks>
/// Implementações desta interface devem fornecer mecanismos de cache,
/// podendo ser em memória, Redis, ou outro provedor de cache.
/// </remarks>
public interface ICacheService
{
    /// <summary>
    /// Armazena um valor no cache associado à chave informada.
    /// </summary>
    /// <param name="key">Chave única para identificar o valor no cache.</param>
    /// <param name="value">Valor a ser armazenado.</param>
    /// <returns>Uma tarefa assíncrona que representa a operação.</returns>
    Task SetValueAsync(string key, string value);

    /// <summary>
    /// Recupera um valor do cache associado à chave informada.
    /// </summary>
    /// <param name="key">Chave única utilizada para buscar o valor no cache.</param>
    /// <returns>
    /// Uma tarefa assíncrona que retorna o valor armazenado ou <c>null</c> caso a chave não exista.
    /// </returns>
    Task<string> GetValueAsync(string key);

    /// <summary>
    /// Remove um valor do cache associado à chave informada.
    /// </summary>
    /// <param name="key">Chave única do valor a ser removido.</param>
    /// <returns>Uma tarefa assíncrona que representa a operação de remoção.</returns>
    Task RemoveValueAsync(string key);
}
