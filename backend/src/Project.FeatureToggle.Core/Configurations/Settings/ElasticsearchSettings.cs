namespace Project.FeatureToggle.Core.Configurations.Settings;

public record ElasticsearchSettings
{
    public string Endpoint { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string IndexPattern { get; set; }
}
