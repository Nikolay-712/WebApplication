using Blazored.LocalStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApp.Client.Services.Interfaces;

namespace WebApp.Client.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly ILocalStorageService _localStorage;

    private readonly JwtSecurityTokenHandler _tokenHandler;

    public TokenService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<string> GetAsync(string key)
    {
        string token = await _localStorage.GetItemAsStringAsync(key);
        return token;
    }

    public async Task SetAsync(string key, string token)
    {
        await _localStorage.SetItemAsStringAsync(key, token);
    }

    public async Task RemoveAsync(string key)
    {
        await _localStorage.RemoveItemAsync(key);
    }

    public IEnumerable<Claim> ReadToken(string token)
    {
        JwtSecurityToken securityToken = _tokenHandler.ReadJwtToken(token);
        return securityToken.Claims;
    }
}
