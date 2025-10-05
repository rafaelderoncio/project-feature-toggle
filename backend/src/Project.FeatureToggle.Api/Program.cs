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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()    // permite todas as origens
              .AllowAnyHeader()    // permite qualquer header
              .AllowAnyMethod();   // permite qualquer método HTTP (GET, POST, etc.)
    });
});

var app = builder.Build();

app.UseCors();

app.UseMiddleware<GlobalErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseHsts();

app.UseSwaggerPage();

app.UseRouting();

app.MapControllers();

app.Run();
