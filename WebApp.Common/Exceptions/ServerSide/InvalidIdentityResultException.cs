namespace WebApp.Common.Exceptions.ServerSide;

public class InvalidIdentityResultException : ServerException
{
    public InvalidIdentityResultException(string? message) 
        : base(message)
    {
    }
}
