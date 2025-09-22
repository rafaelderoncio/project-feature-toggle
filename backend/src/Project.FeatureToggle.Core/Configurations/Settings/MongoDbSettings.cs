namespace Project.FeatureToggle.Core.Configurations.Settings;

public record class MongoDbSettings
{
    public string Database { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Cluster { get; set; }
    public string Domain { get; set; }
    public string ConnectionString => $"mongodb+srv://{User}:{Password}@{Cluster.ToLower()}.{Domain}.mongodb.net/?retryWrites=true&w=majority&appName={Cluster.ToLower()}";
    public string CollectionName { get; set; }
}