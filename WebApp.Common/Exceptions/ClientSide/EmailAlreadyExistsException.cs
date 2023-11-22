namespace WebApp.Common.Exceptions.ClientSide;

public class EmailAlreadyExistsException : ClientException
{
    public EmailAlreadyExistsException(string? message) 
        : base(message)
    {
    }
}
