namespace WebApp.Common.Exceptions.ServerSide;

public class NotSuccessfulRegistrationException : ServerException
{
    public NotSuccessfulRegistrationException(string? message)
        : base(message)
    {
    }
}
