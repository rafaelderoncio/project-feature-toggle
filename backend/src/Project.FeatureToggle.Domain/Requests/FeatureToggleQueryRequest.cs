namespace Project.FeatureToggle.Domain.Requests;

public record FeatureToggleQueryRequest
{
    bool OnlyActive { get; set; } = false;
    int Page { get; set; } = 1;
    int Quantity { get; set; } = 10;
}
