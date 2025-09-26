using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Project.FeatureToggle.Core.Configurations.Settings;
using Project.FeatureToggle.Core.Models;
using Project.FeatureToggle.Core.Repositories.Interfaces;

namespace Project.FeatureToggle.Core.Repositories;

public sealed class FeatureRepository : IFeatureRepository
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<FeatureModel> _collection;
    private readonly MongoDbSettings _settings;

    public FeatureRepository(IMongoDatabase database, IOptions<MongoDbSettings> options)
    {
        _database = database;
        _settings = options.Value;
        _collection = _database.GetCollection<FeatureModel>(_settings.CollectionName);

        var indexKeys = Builders<FeatureModel>.IndexKeys.Ascending(x => x.Feature);
        var indexOptions = new CreateIndexOptions { Unique = true };
        var indexModel = new CreateIndexModel<FeatureModel>(indexKeys, indexOptions);

        _collection.Indexes.CreateOne(indexModel);

        // remove in production
        // InsertSeedData().GetAwaiter().GetResult();
    }
    
    public async Task<FeatureModel> GetFeature(Guid id)
    {
        var filter = Builders<FeatureModel>.Filter.Eq(x => x.Id, id);
        var result = await _collection.Find(filter).FirstOrDefaultAsync();

        return result;
    }

    public async Task<FeatureModel> GetFeature(string feature)
    {
        var filter = Builders<FeatureModel>.Filter.Eq(x => x.Feature, feature);
        var result = await _collection.Find(filter).FirstOrDefaultAsync();

        return result;
    }

    public async Task<FeatureModel[]> GetFeatures()
    {
        var filter = Builders<FeatureModel>.Filter.Empty;
        var result = await _collection.Find(filter).ToListAsync().ContinueWith(t => t.Result);

        return [.. result];
    }

    public async Task<FeatureModel[]> GetFeatures(bool onlyActive, int quantity, int page)
    {
        var filter = onlyActive ?
            Builders<FeatureModel>.Filter.Eq(x => x.Active, true) :
            Builders<FeatureModel>.Filter.Empty;

        var result = await _collection
            .Find(filter)
            .Skip((page - 1) * quantity)
            .Limit(quantity)
            .ToListAsync();
        
        return [.. result];
    }

    public async Task<long> GetTotalFeatures(bool onlyActive)
    {
        var filter = onlyActive ?
            Builders<FeatureModel>.Filter.Eq(x => x.Active, true) :
            Builders<FeatureModel>.Filter.Empty;

        var result = await _collection.CountDocumentsAsync(filter);

        return result;
    }

    public async Task<FeatureModel> SaveFeature(FeatureModel model)
    {
        model.Id = Guid.NewGuid();
        model.CreatedAt = DateTime.UtcNow;
        model.UpdatedAt = model.CreatedAt;
        await _collection.InsertOneAsync(model);

        return model;
    }

    public async Task<FeatureModel> UpdateFeature(FeatureModel model)
    {
        var filter = Builders<FeatureModel>.Filter.Eq(x => x.Id, model.Id);

        var update = Builders<FeatureModel>.Update.Set(x => x.Name, model.Name)
                                                  .Set(x => x.Description, model.Description)
                                                  .Set(x => x.Feature, model.Feature)
                                                  .Set(x => x.Tags, model.Tags)
                                                  .Set(x => x.Active, model.Active)
                                                  .Set(x => x.UpdatedAt, DateTime.UtcNow);

        var options = new FindOneAndUpdateOptions<FeatureModel> { ReturnDocument = ReturnDocument.After };

        var result = await _collection.FindOneAndUpdateAsync(filter, update, options);

        return result;
    }

    public async Task<FeatureModel> UpdateFeature(Guid id, Expression<Func<FeatureModel, object>> field, object value)
    {
        var filter = Builders<FeatureModel>.Filter.Eq(x => x.Id, id);

        var update = Builders<FeatureModel>.Update
                                        .Set(field, value)
                                        .Set(x => x.UpdatedAt, DateTime.UtcNow);

        var options = new FindOneAndUpdateOptions<FeatureModel>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await _collection.FindOneAndUpdateAsync(filter, update, options);
    }

    public async Task<FeatureModel> DeleteFeature(Guid id)
    {
        var filter = Builders<FeatureModel>.Filter.Eq(x => x.Id, id);
        var result = await _collection.FindOneAndDeleteAsync(filter);

        return result;
    }

    private async Task InsertSeedData()
    {
        await _collection.InsertManyAsync(new List<FeatureModel>
            {
                new FeatureModel
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
                new FeatureModel
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
                new FeatureModel
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
                new FeatureModel
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
                new FeatureModel
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
                new FeatureModel
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
                new FeatureModel
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
                new FeatureModel
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
                new FeatureModel
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
                new FeatureModel
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
