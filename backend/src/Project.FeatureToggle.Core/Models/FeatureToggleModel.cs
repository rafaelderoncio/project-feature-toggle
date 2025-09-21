using System;

namespace Project.FeatureToggle.Core.Models;

public record FeatureToggleModel
{
    public Guid Id { get; set; }
    public string Feature { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string[] Tags { get; set; }
    public bool Active { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
