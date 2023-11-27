namespace WebApp.Common.Exceptions.ServerSide;

public class InvalidEmailConfirmationException : ServerException
{
    public InvalidEmailConfirmationException(string? message) 
        : base(message)
    {
    }
}
