namespace WebApp.Common.Exceptions.ClientSide;

public class NotFoundRoleException : ClientException
{
    public NotFoundRoleException(string? message)
        : base(message)
    {
    }
}
