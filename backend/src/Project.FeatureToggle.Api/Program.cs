using Project.FeatureToggle.Core.Configurations.Settings;
using Project.FeatureToggle.Core.Extensions;
using Project.FeatureToggle.Core.Middlewares;
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

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseHsts();

app.UseSwaggerPage();

app.UseRouting();

app.MapControllers();

app.Run();
