namespace Project.FeatureToggle.Core.Services.Interfaces;

/// <summary>
/// Interface para operações de consulta e alteração do estado de feature toggles.
/// </summary>
/// <remarks>
/// Esta interface permite verificar se uma feature está ativa e alterar seu estado
/// diretamente, sendo útil para gerenciamento rápido de toggles.
/// </remarks>
public interface IFeatureToggleService
{
    /// <summary>
    /// Recupera o estado atual de uma feature toggle.
    /// </summary>
    /// <param name="feature">O nome ou chave da feature a ser consultada.</param>
    /// <returns>
    /// Uma <see cref="Task{Boolean}"/> indicando se a feature está ativa (<c>true</c>) ou inativa (<c>false</c>).
    /// </returns>
    Task<bool> GetToggle(string feature);

    /// <summary>
    /// Alterna o estado de uma feature toggle.
    /// </summary>
    /// <param name="feature">O nome ou chave da feature a ser alterada.</param>
    /// <returns>
    /// Uma <see cref="Task{Boolean}"/> indicando o novo estado da feature após a alteração
    /// (<c>true</c> para ativa, <c>false</c> para inativa).
    /// </returns>
    Task<bool> PutToggle(string feature);
}
