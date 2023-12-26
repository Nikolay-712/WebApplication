using WebApp.Models.Response.Roles;

namespace WebApp.Models.Response.Users;

public class UserResponseModel
{
    public Guid UserId { get; set; }

    public string CreatedOn { get; set; }

    public string UserName { get; set; }

    public string Email { get; set; }

    public bool IsConfirmedEmail { get; set; }

    public IReadOnlyList<RoleResponseModel> Roles { get; set; }
}
