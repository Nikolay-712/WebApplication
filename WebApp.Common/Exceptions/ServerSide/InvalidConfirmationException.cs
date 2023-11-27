namespace WebApp.Common.Exceptions.ServerSide;

public class InvalidConfirmationException : ServerException
{
    public InvalidConfirmationException(string? message)
        : base(message)
    {
    }
}
