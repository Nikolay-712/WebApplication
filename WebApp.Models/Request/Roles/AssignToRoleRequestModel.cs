namespace WebApp.Models.Request.Roles;

public class AssignToRoleRequestModel
{
    public Guid UserId { get; set; }

    public Guid RoleId { get; set; }
}
