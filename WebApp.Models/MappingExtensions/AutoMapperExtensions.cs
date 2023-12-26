using WebApp.Data.Entities;
using WebApp.Models.Response;
using WebApp.Models.Response.Roles;
using WebApp.Models.Response.Users;

namespace WebApp.Models.MappingExtensions;

public static class AutoMapperExtensions
{
    public static UserProfileResponseModel ToUserProfileResponseModel(this ApplicationUser user)
    {
        return new UserProfileResponseModel
        {
            UserName = user.UserName!,
            Email = user.Email!,
            CreatedOn = user.CreatedOn.ToString("dd.MM.yyyy"),
        };
    }

    public static RoleResponseModel ToRoleResponseModel(this ApplicationRole role)
    {
        return new RoleResponseModel
        {
            Id = role.Id,
            Name = role.Name!,
            CreatedOn = role.CreatedOn.ToString("dd.MM.yyyy"),
            DescriptionBg = role.DescriptionBg,
            DescriptionEn = role.DescriptionEn,
        };
    }

    public static UserResponseModel ToUserResponseModel(this ApplicationUser user, IReadOnlyList<RoleResponseModel> roles)
    {
        return new UserResponseModel
        {
            UserId = user.Id,
            CreatedOn = user.CreatedOn.ToString("dd.MM.yyyy"),
            UserName = user.UserName!,
            Email = user.Email!,
            IsConfirmedEmail = user.EmailConfirmed,
            Roles = roles,
        };
    }
}
