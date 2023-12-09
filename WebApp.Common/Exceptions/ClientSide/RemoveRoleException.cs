namespace WebApp.Common.Exceptions.ClientSide;

public class RemoveRoleException : ClientException
{
    public RemoveRoleException(string? message)
        : base(message)
    {
    }
}
