
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Project.FeatureToggle.Core.Configurations.Settings;
using Project.FeatureToggle.Core.Repositories;
using Project.FeatureToggle.Core.Repositories.Interfaces;
using Project.FeatureToggle.Core.Services;
using Project.FeatureToggle.Core.Services.Interfaces;

namespace Project.FeatureToggle.Core.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IFeatureManagerService, FeatureManagerService>();
        services.AddSingleton<IFeatureToggleService, FeatureToggleService>();
        services.AddSingleton<IFeatureDashboardService, FeatureDashboardService>();
        services.AddSingleton<ICacheService, CacheService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IFeatureRepository, FeatureRepository>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MongoDbSettings>>();
            var settings = MongoClientSettings.FromConnectionString(options.Value.ConnectionString);
            var client = new MongoClient(settings);
            var database = client.GetDatabase(options.Value.Database);
            return new(database, options);
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Program:SwaggerSettings").Get<SwaggerSettings>()
            ?? throw new InvalidOperationException("SwaggerSettings has not configured!");

        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc(settings.Version, new OpenApiInfo
            {
                Title = settings.Title,
                Version = settings.Version,
                Description = settings.Description,
                Contact = new OpenApiContact
                {
                    Name = settings.Contact?.Name,
                    Email = settings.Contact?.Email,
                    Url = string.IsNullOrEmpty(settings.Contact?.Url)
                        ? null : new Uri(settings.Contact.Url)
                },
                License = new OpenApiLicense
                {
                    Name = settings.License?.Name,
                    Url = string.IsNullOrEmpty(settings.License?.Url)
                        ? null : new Uri(settings.License.Url)
                }
            });
        });

        return services;
    }
}
