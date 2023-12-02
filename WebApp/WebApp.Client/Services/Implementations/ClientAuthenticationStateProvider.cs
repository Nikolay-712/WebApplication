using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;
using WebApp.Client.Services.Interfaces;

namespace WebApp.Client.Services.Implementations;

public class ClientAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;
    private readonly string _tokenKey = "jwt_token";

    public ClientAuthenticationStateProvider(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = null;
        //var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJjYjhjOTE4Yi1lNGFhLTQzNzQtYWNjNC0wOGRiZWZmYzAyMWMiLCJlbWFpbCI6Im4uZ2VvcmdpZXZwZXJzb25hbEBnbWFpbC5jb20iLCJuYmYiOjE3MDEzNzAzMzIsImV4cCI6MTcwMTM3MzkzMiwiaWF0IjoxNzAxMzcwMzMyLCJpc3MiOiJodHRwczovL2xvY2FsaG9zdDo3MDU2IiwiYXVkIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzA1NiJ9.MJ5kqM52JVpW2ytRcfOajS8GYbEe3PLr5xi8UDB1EW0";
        //var token = await _tokenService.GetAsync(_tokenKey);

        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }


      var claims =  ParseClaimsFromJwt(token);

        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        return await authState;
    }

    public void MarkUserAsAuthenticated(string email)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, email) }, "apiauth"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        var authState = Task.FromResult(new AuthenticationState(anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var claims = new List<Claim>();
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);

        if (roles != null)
        {
            if (roles.ToString().Trim().StartsWith("["))
            {
                var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                foreach (var parsedRole in parsedRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                }
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
            }

            keyValuePairs.Remove(ClaimTypes.Role);
        }

        claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));

        return claims;
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }
}
