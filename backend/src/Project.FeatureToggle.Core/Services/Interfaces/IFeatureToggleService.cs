namespace Project.FeatureToggle.Core.Services.Interfaces;

public interface IFeatureToggleService
{
    Task<bool> GetToggle(string feature);
    Task<bool> PutToggle(string feature);
}
