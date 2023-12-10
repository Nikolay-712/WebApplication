using WebApp.Data.Entities;

namespace WebApp.Services.Interfaces;

public interface IUserService
{
    Task<ApplicationUser> GetByIdAsync(Guid userId);
}
