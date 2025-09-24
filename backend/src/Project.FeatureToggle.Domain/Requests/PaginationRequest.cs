namespace Project.FeatureToggle.Domain.Requests;

public record PaginationRequest
{
    public bool OnlyActive { get; set; } = false;
    public int Page { get; set; } = 1;
    public int Quantity { get; set; } = 10;
}
