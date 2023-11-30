namespace WebApp.Client.Services.Interfaces;

public interface ITokenService
{
    Task<string> GetAsync(string key);
    Task SetAsync(string key, string token);
    Task RemoveAsync(string key);
}
