namespace WebApp.Common.Exceptions.ClientSide;

public class ExistsRoleException : ClientException
{
    public ExistsRoleException(string? message)
        : base(message)
    {
    }
}
