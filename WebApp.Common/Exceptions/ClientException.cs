using System.Net;

namespace WebApp.Common.Exceptions;

public class ClientException : Exception
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public ClientException(string? message)
        : base(message)
    {
    }
}
