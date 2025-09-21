using System;

namespace Project.FeatureToggle.Domain.Responses;

public record FeatureToggleResponse
{
    public string Id { get; set; }
    public string Feature { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Tags { get; set; } = [];
    public bool Active { get; set; }
}
