using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Project.FeatureToggle.Core.Configurations.Settings;
using Project.FeatureToggle.Core.Models;
using Project.FeatureToggle.Core.Repositories.Interfaces;

namespace Project.FeatureToggle.Core.Repositories;

public class FeatureToggleRepository : IFeatureToggleRepository
{
    private readonly IMongoCollection<FeatureToggleModel> _collection;

    public FeatureToggleRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings)
    {
        _collection = database.GetCollection<FeatureToggleModel>(settings.Value.CollectionName);

        var indexKeys = Builders<FeatureToggleModel>.IndexKeys.Ascending(x => x.Feature);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexModel = new CreateIndexModel<FeatureToggleModel>(indexKeys, indexOptions);

        _collection.Indexes.CreateOne(indexModel);
        
        // InsertSeedData().GetAwaiter().GetResult();
    }

    public async Task<FeatureToggleModel> GetFeatureToggle(Guid id)
    {
        var filter = Builders<FeatureToggleModel>.Filter.Eq(x => x.Id, id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<FeatureToggleModel> GetFeatureToggle(string feature)
    {
        var filter = Builders<FeatureToggleModel>.Filter.Eq(x => x.Feature, feature);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<FeatureToggleModel[]> GetFeatureToggle()
    {
        return await _collection.Find(Builders<FeatureToggleModel>.Filter.Empty)
                                .ToListAsync()
                                .ContinueWith(t => t.Result.ToArray());
    }

    public async Task<FeatureToggleModel> SaveFeatureToggle(FeatureToggleModel model)
    {
        model.Id = Guid.NewGuid();
        model.CreatedAt = DateTime.UtcNow;
        model.UpdatedAt = DateTime.UtcNow;

        await _collection.InsertOneAsync(model);
        return model;
    }

    public async Task<FeatureToggleModel> UpdateFeatureToggle(FeatureToggleModel model)
    {
        model.UpdatedAt = DateTime.UtcNow;

        var filter = Builders<FeatureToggleModel>.Filter.Eq(x => x.Id, model.Id);
        var options = new FindOneAndReplaceOptions<FeatureToggleModel>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await _collection.FindOneAndReplaceAsync(filter, model, options);
    }

    public async Task<FeatureToggleModel> DeleteFeatureToggle(Guid id)
    {
        var filter = Builders<FeatureToggleModel>.Filter.Eq(x => x.Id, id);
        return await _collection.FindOneAndDeleteAsync(filter);
    }

    private async Task InsertSeedData()
    {
        if (await _collection.CountDocumentsAsync(Builders<FeatureToggleModel>.Filter.Empty) > 0)
            await _collection.InsertManyAsync(new List<FeatureToggleModel>
            {
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "user-registration",
                    Name = "User Registration",
                    Description = "Enable new user registrations.",
                    Tags = new[] { "user", "auth" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "two-factor-auth",
                    Name = "Two Factor Authentication",
                    Description = "Require 2FA for login.",
                    Tags = new[] { "security", "auth" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "dark-mode",
                    Name = "Dark Mode",
                    Description = "Allow users to switch between light and dark themes.",
                    Tags = new[] { "ui", "theme" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "beta-dashboard",
                    Name = "Beta Dashboard",
                    Description = "Enable new dashboard layout for beta users.",
                    Tags = new[] { "dashboard", "beta" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "notifications",
                    Name = "Notifications",
                    Description = "Enable in-app notifications.",
                    Tags = new[] { "notifications", "user" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "export-csv",
                    Name = "Export Data as CSV",
                    Description = "Allow users to export data in CSV format.",
                    Tags = new[] { "export", "data" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "ai-recommendations",
                    Name = "AI Recommendations",
                    Description = "Show AI-based recommendations to users.",
                    Tags = new[] { "ai", "recommendations" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "payment-gateway",
                    Name = "Payment Gateway",
                    Description = "Enable online payments using multiple providers.",
                    Tags = new[] { "payment", "ecommerce" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "audit-logs",
                    Name = "Audit Logs",
                    Description = "Track all user and system activities.",
                    Tags = new[] { "logs", "security" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureToggleModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "api-access",
                    Name = "API Access",
                    Description = "Allow external applications to access via API keys.",
                    Tags = new[] { "api", "integration" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            });
    }
}
