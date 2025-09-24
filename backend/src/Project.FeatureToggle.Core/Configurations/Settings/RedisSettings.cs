namespace Project.FeatureToggle.Core.Configurations.Settings;

public record RedisSettings
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Endpoint { get; set; }
    public int Port { get; set; }
    public bool Ssl { get; set; }
    public bool AbortConnect { get; set; }
    public string ConnectionString => $"{Endpoint}:{Port},password={Password},ssl={Ssl},abortConnect={AbortConnect}";
    public int ExpiresInMinutes { get; set; }
}
