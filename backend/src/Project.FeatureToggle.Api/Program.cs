using Microsoft.OpenApi.Models;
using Project.FeatureToggle.Core.Configurations.Settings;
using Project.FeatureToggle.Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("Program:MongoDbSettings"))
                .Configure<SwaggerSettings>(builder.Configuration.GetSection("Program:SwaggerSettings"));

builder.Services.AddServices();

builder.Services.AddRepositories();

builder.Services.AddControllers();

builder.Services.AddSwagger(builder.Configuration);

var app = builder.Build();

app.UseSwaggerConfigured();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
