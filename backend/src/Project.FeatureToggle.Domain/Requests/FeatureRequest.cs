namespace Project.FeatureToggle.Domain.Requests;

public record FeatureRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Tags { get; set; } = [];
    public bool Active { get; set; }
}
