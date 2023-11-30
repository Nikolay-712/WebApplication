using Blazored.LocalStorage;
using WebApp.Client.Services.Interfaces;

namespace WebApp.Client.Services.Implementations;

public class TokenService : ITokenService
{
    private readonly ILocalStorageService _localStorage;

    public TokenService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
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
}
