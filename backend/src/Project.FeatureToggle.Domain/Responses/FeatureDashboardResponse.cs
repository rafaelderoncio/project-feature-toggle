namespace Project.FeatureToggle.Domain.Responses;

public record FeatureDashboardResponse
{
    public int TotalActives { get; set; }
    public int TotalInactives { get; set; }
    public int TotalFeatures { get; set; }
}
