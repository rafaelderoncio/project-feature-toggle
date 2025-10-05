using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Project.FeatureToggle.Core.Configurations.Settings;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Project.FeatureToggle.Core.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseSerilog(this IHostBuilder builder, IConfiguration configuration)
    {
        var settings = configuration.GetSection("Program:ElasticsearchSettings").Get<ElasticsearchSettings>()
            ?? throw new InvalidOperationException("ElasticsearchSettings has not configured!");

        Log.Logger = new LoggerConfiguration()
            // .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(settings.Endpoint))
            {
                AutoRegisterTemplate = true,
                IndexFormat = settings.IndexPattern,
                ModifyConnectionSettings = conn => conn.BasicAuthentication(settings.Username, settings.Password)
            }).CreateLogger();

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddSerilog(dispose: true);

        }).UseSerilog();

        return builder;
    }
}
