using WebApp.Data.Entities;

namespace WebApp.Services.Interfaces;

public interface IJwtTokenManager
{
    Task<string> GenerateJwtTokenAsync(ApplicationUser user);

    Task<string> GenerateConfirmEmailTokenAsync(string email);
}
