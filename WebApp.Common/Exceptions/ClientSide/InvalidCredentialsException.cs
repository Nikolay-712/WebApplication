namespace WebApp.Common.Exceptions.ClientSide;

public class InvalidCredentialsException : ClientException
{
    public InvalidCredentialsException(string? message)
        : base(message)
    {
    }
}
