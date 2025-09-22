using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Project.FeatureToggle.Core.Models;

public record FeatureToggleModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    [BsonElement("feature")]
    [BsonRequired]
    public string Feature { get; set; }

    [BsonElement("name")]
    [BsonIgnoreIfNull]
    public string Name { get; set; }

    [BsonElement("description")]
    [BsonIgnoreIfNull]
    public string Description { get; set; }

    [BsonElement("tags")]
    [BsonIgnoreIfNull]
    public string[] Tags { get; set; }

    [BsonElement("active")]
    public bool Active { get; set; }

    [BsonElement("createdAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }

    [BsonElement("updatedAt")]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime UpdatedAt { get; set; }
}
