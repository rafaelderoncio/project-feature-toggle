using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Project.FeatureToggle.Core.Configurations.Enums;
using Project.FeatureToggle.Core.Exceptions;
using Project.FeatureToggle.Domain.Responses;

namespace Project.FeatureToggle.Core.Middlewares;

public class GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error on procces request\n{0}\n{1}", ex.Message, ex.StackTrace);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {   
        var response = new ExceptionResponse { };

        var statusCode = StatusCodes.Status500InternalServerError;

        if (exception is BaseException baseException)
        {
            statusCode = (int)baseException.Code;
            
            response.Title = baseException.Title;
            response.Messages = baseException.Messages;
            response.Type = baseException.Type.ToString();
        }
        else
        {
            response.Title = "Internal Server Error";
            response.Messages = ["Error on process request"];
            response.Type = ErrorType.Fatal.ToString();
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
