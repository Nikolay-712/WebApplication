using WebApp.Data.Entities;
using WebApp.Models.Request.Users;
using WebApp.Models.Response.Users;
using WebApp.Models;

namespace WebApp.Services.Interfaces;

public interface IUserService
{
    Task<ApplicationUser> GetByIdAsync(Guid userId);

    Task<UserResponseModel> GetDetailsByIdAsync(Guid userId);

    Task<PaginationResponseModel<UserResponseModel>> GetAllUsersAsync(UsersFilter usersFilter);
}
