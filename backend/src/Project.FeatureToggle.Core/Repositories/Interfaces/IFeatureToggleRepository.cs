using System;
using Project.FeatureToggle.Core.Models;

namespace Project.FeatureToggle.Core.Repositories.Interfaces;

public interface IFeatureToggleRepository
{
    Task<FeatureToggleModel> GetFeatureToggle(Guid id);
    Task<FeatureToggleModel> GetFeatureToggle(string feature);
    Task<FeatureToggleModel[]> GetFeatureToggle();
    Task<FeatureToggleModel> SaveFeatureToggle(FeatureToggleModel model);
    Task<FeatureToggleModel> UpdateFeatureToggle(FeatureToggleModel model);
    Task<FeatureToggleModel> DeleteFeatureToggle(Guid id);
}
