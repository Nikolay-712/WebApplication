using System.Net;

namespace WebApp.Common.Exceptions;

public class ServerException : Exception
{
    public HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    public ServerException(string? message)
        : base(message)
    {
    }
}
