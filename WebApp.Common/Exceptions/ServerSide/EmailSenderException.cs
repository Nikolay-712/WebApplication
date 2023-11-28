namespace WebApp.Common.Exceptions.ServerSide;

public class EmailSenderException : ServerException
{
    public EmailSenderException(string? message)
        : base(message)
    {
    }
}
