using WebApp.Models.Request.Roles;
using WebApp.Models.Response.Roles;

namespace WebApp.Services.Interfaces;

public interface IRoleService
{
    Task CreateAsync(CreateRoleRequestModel requestModel);

    Task<RoleResponseModel> GetByIdAsync(Guid id);

    Task<IReadOnlyList<RoleResponseModel>> GetAllAsync();

    Task UpdateAsync(Guid id, UpdateRoleRequestModel requestModel);

    Task RemoveAsync(Guid id);
}
