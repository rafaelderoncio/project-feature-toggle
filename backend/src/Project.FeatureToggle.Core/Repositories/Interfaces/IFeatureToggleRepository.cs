using System.Linq.Expressions;
using Project.FeatureToggle.Core.Arguments;
using Project.FeatureToggle.Core.Models;

namespace Project.FeatureToggle.Core.Repositories.Interfaces;

public interface IFeatureRepository
{
    Task<FeatureModel> GetFeature(Guid id);
    Task<FeatureModel> GetFeature(string feature);
    Task<FeatureModel[]> GetFeatures();
    Task<FeatureModel[]> GetFeatures(FeatureArgument argument);
    Task<long> GetTotalFeatures(FeatureArgument argument);
    Task<FeatureModel> SaveFeature(FeatureModel model);
    Task<FeatureModel> UpdateFeature(FeatureModel model);
    Task<FeatureModel> UpdateFeature(Guid id, Expression<Func<FeatureModel, object>> field, object value);
    Task<FeatureModel> DeleteFeature(Guid id);
    Task<(int, int, int)> GetFeatureStatus();
}
