namespace Project.FeatureToggle.Core.Configurations.Settings;

public record class SwaggerSettings
{
    public bool Enable { get; set; } = true;
    public string Title { get; set; }
    public string Description { get; set; }
    public string Version { get; set; }
    public SwaggerContact Contact { get; set; }
    public SwaggerLicense License { get; set; }
    public List<SwaggerServer> Servers { get; set; } = new();
    public bool ShowSchemas { get; set; } = false;
}

public class SwaggerContact
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Url { get; set; }
}

public class SwaggerLicense
{
    public string Name { get; set; }
    public string Url { get; set; }
}

public class SwaggerServer
{
    public string Url { get; set; }
    public string Description { get; set; }
}
