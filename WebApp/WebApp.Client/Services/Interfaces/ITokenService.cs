using System.Security.Claims;
using WebApp.Common.Configurations;

namespace WebApp.Client.Services.Interfaces;

public interface ITokenService
{
    Task<string> GetAsync(string key);

    Task SetAsync(string key, string token);

    Task RemoveAsync(string key);

    IEnumerable<Claim> ReadToken(string token);
}
