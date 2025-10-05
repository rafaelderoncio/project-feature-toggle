using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Misc;
using Project.FeatureToggle.Core.Arguments;
using Project.FeatureToggle.Core.Configurations.Settings;
using Project.FeatureToggle.Core.Models;
using Project.FeatureToggle.Core.Repositories.Interfaces;
using Project.FeatureToggle.Domain.Constants;

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

    public async Task<FeatureModel[]> GetFeatures(FeatureArgument argument)
    {
        var filter = Builders<FeatureModel>.Filter.Empty;

        if (argument.Filter == FeatureFilter.ACTIVE)
        {
            filter = Builders<FeatureModel>.Filter.Eq(x => x.Active, true);
        }
        else if (argument.Filter == FeatureFilter.INACTIVE)
        {
            filter = Builders<FeatureModel>.Filter.Eq(x => x.Active, false);
        }

        if (!string.IsNullOrWhiteSpace(argument.Search))
        {
            var searchFilter = Builders<FeatureModel>.Filter.Regex(x => x.Name, new BsonRegularExpression(argument.Search, "i"));
            filter = Builders<FeatureModel>.Filter.And(filter, searchFilter);
        }

        var page = argument.Page > 0 ? argument.Page : 1;
        var quantity = argument.Quantity > 0 ? argument.Quantity : 10;

        var result = await _collection
            .Find(filter)
            .Skip((page - 1) * quantity)
            .Limit(quantity)
            .ToListAsync();

        return result.ToArray();
    }

    public async Task<long> GetTotalFeatures(FeatureArgument argument)
    {
        var filter = Builders<FeatureModel>.Filter.Empty;

        if (argument.Filter == FeatureFilter.ACTIVE)
        {
            filter = Builders<FeatureModel>.Filter.Eq(x => x.Active, true);
        }
        else if (argument.Filter == FeatureFilter.INACTIVE)
        {
            filter = Builders<FeatureModel>.Filter.Eq(x => x.Active, false);
        }

        if (!string.IsNullOrWhiteSpace(argument.Search))
        {
            var searchFilter = Builders<FeatureModel>.Filter.Regex(x => x.Name, new BsonRegularExpression(argument.Search, "i"));
            filter = Builders<FeatureModel>.Filter.And(filter, searchFilter);
        }

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
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "rate-limiting",
                    Name = "Rate Limiting",
                    Description = "Limit the number of API requests per user.",
                    Tags = new[] { "api", "security", "performance" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "data-encryption",
                    Name = "Data Encryption",
                    Description = "Encrypt sensitive user data at rest and in transit.",
                    Tags = new[] { "security", "compliance" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "role-based-access",
                    Name = "Role-Based Access Control",
                    Description = "Manage access permissions based on user roles.",
                    Tags = new[] { "auth", "security" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "email-verification",
                    Name = "Email Verification",
                    Description = "Require email verification during sign-up.",
                    Tags = new[] { "auth", "user" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "webhooks",
                    Name = "Webhooks",
                    Description = "Enable outbound webhooks for integrations.",
                    Tags = new[] { "api", "integration" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "search-optimization",
                    Name = "Search Optimization",
                    Description = "Enhance the speed and relevance of search results.",
                    Tags = new[] { "search", "performance" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "file-upload",
                    Name = "File Upload",
                    Description = "Allow users to upload and manage files securely.",
                    Tags = new[] { "storage", "ui" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "report-builder",
                    Name = "Custom Report Builder",
                    Description = "Enable users to build custom reports with filters.",
                    Tags = new[] { "report", "analytics" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "multi-language",
                    Name = "Multi-Language Support",
                    Description = "Provide support for multiple languages in the UI.",
                    Tags = new[] { "ui", "i18n" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "session-timeout",
                    Name = "Session Timeout",
                    Description = "Automatically log out inactive users after a set period.",
                    Tags = new[] { "security", "auth" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "ab-testing",
                    Name = "A/B Testing",
                    Description = "Run experiments to test feature performance.",
                    Tags = new[] { "testing", "ui", "analytics" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "live-chat-support",
                    Name = "Live Chat Support",
                    Description = "Provide real-time support through live chat.",
                    Tags = new[] { "support", "ui" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "usage-analytics",
                    Name = "Usage Analytics",
                    Description = "Track and analyze user activity in the platform.",
                    Tags = new[] { "analytics", "insights" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "captcha-login",
                    Name = "Captcha on Login",
                    Description = "Add CAPTCHA verification to prevent bot logins.",
                    Tags = new[] { "security", "auth" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "mobile-push-notifications",
                    Name = "Mobile Push Notifications",
                    Description = "Send push notifications to mobile app users.",
                    Tags = new[] { "notifications", "mobile" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "offline-mode",
                    Name = "Offline Mode",
                    Description = "Allow app to work with limited features while offline.",
                    Tags = new[] { "mobile", "performance" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "biometric-login",
                    Name = "Biometric Login",
                    Description = "Allow login using fingerprint or face recognition.",
                    Tags = new[] { "auth", "security", "mobile" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "custom-themes",
                    Name = "Custom Themes",
                    Description = "Enable users to create and apply their own color themes.",
                    Tags = new[] { "ui", "theme", "customization" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "drag-drop-dashboard",
                    Name = "Drag & Drop Dashboard",
                    Description = "Allow users to customize dashboards via drag and drop.",
                    Tags = new[] { "ui", "dashboard", "customization" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "auto-backup",
                    Name = "Automatic Backups",
                    Description = "Schedule automatic backups of all data.",
                    Tags = new[] { "storage", "security", "performance" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "single-sign-on",
                    Name = "Single Sign-On (SSO)",
                    Description = "Enable users to log in with external identity providers (e.g., Google, Azure AD).",
                    Tags = new[] { "auth", "security", "integration" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "custom-webhooks",
                    Name = "Custom Webhooks",
                    Description = "Allow users to define custom webhook triggers.",
                    Tags = new[] { "api", "integration", "automation" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "auto-scaling",
                    Name = "Auto Scaling",
                    Description = "Automatically scale resources based on usage demand.",
                    Tags = new[] { "performance", "cloud" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "geo-location",
                    Name = "Geo-Location Services",
                    Description = "Enable location-based personalization and analytics.",
                    Tags = new[] { "mobile", "analytics", "personalization" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "video-tutorials",
                    Name = "Video Tutorials",
                    Description = "Provide integrated onboarding video tutorials.",
                    Tags = new[] { "user", "onboarding", "ui" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "smart-search",
                    Name = "Smart Search",
                    Description = "Use AI to improve search accuracy and suggestions.",
                    Tags = new[] { "search", "ai", "performance" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "multi-factor-approval",
                    Name = "Multi-Factor Approval",
                    Description = "Require multiple approvers for sensitive actions.",
                    Tags = new[] { "security", "workflow" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "team-collaboration",
                    Name = "Team Collaboration",
                    Description = "Allow teams to collaborate in real time on shared projects.",
                    Tags = new[] { "collaboration", "ui", "productivity" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "scheduled-messages",
                    Name = "Scheduled Messages",
                    Description = "Allow users to schedule messages or notifications.",
                    Tags = new[] { "user", "notifications", "automation" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "advanced-reporting",
                    Name = "Advanced Reporting",
                    Description = "Provide deeper data insights with advanced charts.",
                    Tags = new[] { "analytics", "report", "ui" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "fraud-detection",
                    Name = "Fraud Detection",
                    Description = "Detect unusual or fraudulent user activities.",
                    Tags = new[] { "security", "ai", "compliance" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "voice-commands",
                    Name = "Voice Commands",
                    Description = "Allow voice-activated commands for navigation.",
                    Tags = new[] { "ui", "accessibility", "mobile" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "custom-alerts",
                    Name = "Custom Alerts",
                    Description = "Let users configure their own alerts and triggers.",
                    Tags = new[] { "user", "notifications", "automation" },
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new FeatureModel
                {
                    Id = Guid.NewGuid(),
                    Feature = "incident-management",
                    Name = "Incident Management",
                    Description = "Centralize reporting and tracking of incidents.",
                    Tags = new[] { "operations", "security", "workflow" },
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
        });
    }

    public async Task<(int, int, int)> GetFeatureStatus()
    {
        var pipeline = new[]
        {
            new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$active" },
                { "count", new BsonDocument("$sum", 1) }
            })
        };

        var results = await _collection.Aggregate<BsonDocument>(pipeline).ToListAsync();

        int total = 0, active = 0, inactive = 0;

        foreach (var result in results)
        {
            bool isActive = result["_id"].AsBoolean;
            int count = result["count"].AsInt32;

            total += count;
            if (isActive)
                active = count;
            else
                inactive = count;
        }

        return (total, active, inactive);
    }
}
