using WebApp.Data.Entities;
using WebApp.Models.Response;

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
}
