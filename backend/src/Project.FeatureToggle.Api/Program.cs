using Microsoft.OpenApi.Models;
using Project.FeatureToggle.Core.Repositories;
using Project.FeatureToggle.Core.Repositories.Interfaces;
using Project.FeatureToggle.Core.Services;
using Project.FeatureToggle.Core.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IFeatureToggleService, FeatureToggleService>();

builder.Services.AddTransient<IFeatureToggleRepository, FeatureToggleRepository>();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "",
        Version = "v1",
        Description = ""
    });
});

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "");
    c.RoutePrefix = "api/swagger";
    c.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
