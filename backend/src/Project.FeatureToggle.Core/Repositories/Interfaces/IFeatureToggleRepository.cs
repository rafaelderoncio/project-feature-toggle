using System;
using Project.FeatureToggle.Core.Models;

namespace Project.FeatureToggle.Core.Repositories.Interfaces;

public interface IFeatureRepository
{
    Task<FeatureModel> GetFeature(Guid id);
    Task<FeatureModel> GetFeature(string feature);
    Task<FeatureModel[]> GetFeatures();
    Task<FeatureModel[]> GetFeatures(bool onlyActive, int quantity, int page);
    Task<long> GetTotalFeatures(bool onlyActive);
    Task<FeatureModel> SaveFeature(FeatureModel model);
    Task<FeatureModel> UpdateFeature(FeatureModel model);
    Task<FeatureModel> DeleteFeature(Guid id);
}
