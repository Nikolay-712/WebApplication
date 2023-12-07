using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using WebApp.Client.Services.Implementations;
using WebApp.Client.Services.Interfaces;
using static WebApp.Common.Constants;

namespace WebApp.Client.Services;

public class ClientAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;
    private readonly HttpClient httpClient;
    private readonly ClaimsPrincipal _anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

    public ClientAuthenticationStateProvider(ITokenService tokenService,HttpClient httpClient)
    {
        _tokenService = tokenService;
        this.httpClient = httpClient;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = await _tokenService.GetAsync(TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(_anonymousUser);
        }

        IEnumerable<Claim> claims = _tokenService.ReadToken(token);
        if (claims is null)
        {
            return new AuthenticationState(_anonymousUser);
        }

        ClaimsPrincipal authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
        Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        httpClient.AddJwtToken(token);
        return await authState;
    }

    public void MarkUserAsAuthenticated(string token)
    {
        IEnumerable<Claim> claims = _tokenService.ReadToken(token);
        if (claims is not null)
        {
            ClaimsPrincipal authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
            Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }

    public void MarkUserAsLoggedOut()
    {
        Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(_anonymousUser));
        NotifyAuthenticationStateChanged(authState);
    }

}