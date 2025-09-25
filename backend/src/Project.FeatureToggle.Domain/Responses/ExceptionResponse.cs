namespace Project.FeatureToggle.Domain.Responses;

public record ExceptionResponse
{
    public string Type { get; set; }
    public string Title { get; set; }
    public string[] Messages { get; set; }
}
