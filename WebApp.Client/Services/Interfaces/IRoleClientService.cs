using WebApp.Models.Request.Roles;
using WebApp.Models;
using WebApp.Models.Response.Roles;

namespace WebApp.Client.Services.Interfaces;

public interface IRoleClientService
{
    Task<ResponseContent> CreateAsync(CreateRoleRequestModel requestModel);

    Task<ResponseContent<IReadOnlyList<RoleResponseModel>>> GetAllAsync();

    Task<ResponseContent<RoleResponseModel>> GetDetailsAsync(Guid id);

    Task<ResponseContent> RemoveAsync(Guid id);
}
