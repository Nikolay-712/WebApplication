namespace WebApp.Common.Exceptions.ClientSide;

public class NotFoundUserException : ClientException
{
    public NotFoundUserException(string? message)
        : base(message)
    {
    }
}
