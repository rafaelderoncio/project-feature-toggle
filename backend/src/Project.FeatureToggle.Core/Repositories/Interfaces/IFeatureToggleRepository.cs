using System.Linq.Expressions;
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
    Task<FeatureModel> UpdateFeature(Guid id, Expression<Func<FeatureModel, object>> field, object value);
    Task<FeatureModel> DeleteFeature(Guid id);
}
