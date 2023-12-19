using WebApp.Models;
using WebApp.Models.Request.Users;
using WebApp.Models.Response.Users;

namespace WebApp.Client.Services.Interfaces;

public interface IUserClientService
{
    Task<ResponseContent<PaginationResponseModel<UserResponseModel>>> GetAllAsync(UsersFilter usersFilter);
}
