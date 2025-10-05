using Project.FeatureToggle.Domain.Constants;

namespace Project.FeatureToggle.Domain.Requests;

public record FeatureQueryRequest
{
    private int _page = 1;
    private int _quantity = 6;
    public string Filter { get; set; } = FeatureFilter.ALL;
    public string Search { get; set; }
    public int Page
    {
        get => _page;
        set => _page = value < 1 ? _page : value;
    }
    public int Quantity
    {
        get => _quantity;
        set => _quantity = value < 1 || value > 6 ? _quantity : value;
    }
}
