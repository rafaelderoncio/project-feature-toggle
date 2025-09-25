using Project.FeatureToggle.Core.Configurations.Settings;
using Project.FeatureToggle.Core.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("Program:MongoDbSettings"))
                .Configure<SwaggerSettings>(builder.Configuration.GetSection("Program:SwaggerSettings"))
                .Configure<RedisSettings>(builder.Configuration.GetSection("Program:RedisSettings"));

builder.Host.UseSerilog(builder.Configuration);

builder.Services.AddServices();

builder.Services.AddRepositories();

builder.Services.AddControllers();

builder.Services.AddSwagger(builder.Configuration);

var app = builder.Build();

app.UseSwaggerPage();

app.UseHttpsRedirection().UseHsts();

app.MapControllers();

app.Run();
