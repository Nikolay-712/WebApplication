namespace WebApp.Common.Exceptions.ClientSide;

public class UserRoleExistsException : ClientException
{
    public UserRoleExistsException(string? message) 
        : base(message)
    {
    }
}
