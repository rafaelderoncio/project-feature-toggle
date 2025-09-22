using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Project.FeatureToggle.Core.Configurations.Settings;

namespace Project.FeatureToggle.Core.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerConfigured(this IApplicationBuilder app)
    {
        var settings = app.ApplicationServices.GetRequiredService<IOptions<SwaggerSettings>>().Value;

        if (!settings.Enable)
            return app;

        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint($"/swagger/{settings.Version}/swagger.json", settings.Title);

            options.DocumentTitle = settings.Title;

            if (!settings.ShowSchemas)
                options.DefaultModelsExpandDepth(-1);

            if (settings.Servers?.Any() == true)
                foreach (var server in settings.Servers)
                {
                    options.SwaggerEndpoint(
                        $"{server.Url}/swagger/{settings.Version}/swagger.json",
                        server.Description ?? server.Url
                    );
                }
        });

        return app;
    }
}
