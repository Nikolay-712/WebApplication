namespace WebApp.Common.Exceptions.ClientSide;

public class UserNotInRoleException : ClientException
{
    public UserNotInRoleException(string? message) 
        : base(message)
    {
    }
}
