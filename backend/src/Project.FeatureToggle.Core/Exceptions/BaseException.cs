using System.Net;
using Project.FeatureToggle.Core.Configurations.Enums;

namespace Project.FeatureToggle.Core.Exceptions;

public class BaseException : Exception
{
    public ErrorType Type { get; }
    public string Title { get; }
    public string[] Messages { get; }
    public HttpStatusCode Code { get; }

    public BaseException(string title, string message, HttpStatusCode code = HttpStatusCode.UnprocessableEntity) : base(message)
    {
        Title = title;
        Messages = [message];
        Type = ErrorType.ServerError;
        Code = code;
    }

    public BaseException(string title, string message, ErrorType type, HttpStatusCode code = HttpStatusCode.UnprocessableEntity) : base(message)
    {
        Title = title;
        Messages = [message];
        Type = type;
        Code = code;
    }

    public BaseException(string title, string message, Exception ex) : base(message, ex)
    {
        Title = title;
        Messages = [message, ex.Message, ex.StackTrace];
        Type = ErrorType.ServerError;
        Code = HttpStatusCode.InternalServerError;
    }
}
