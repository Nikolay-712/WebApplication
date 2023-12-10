namespace WebApp.Models.Request.Roles;

public class RemoveFromRoleRequestModel
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
}
