using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using WebApp.Client.Services.Interfaces;
using Microsoft.Extensions.Options;
using WebApp.Common.Configurations;
using static WebApp.Common.Constants;

namespace WebApp.Client.Services.Implementations;

public class ClientAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;
    private readonly ClaimsPrincipal _anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

    public ClientAuthenticationStateProvider(ITokenService tokenService, IOptions<JwtTokenSettings> options)
    {
        _tokenService = tokenService;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        Task<string> tokenTask = await Task.FromResult(_tokenService.GetAsync(TokenKey));
        if (tokenTask.Exception is null)
        {
            string token = await tokenTask;
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
            return await authState;
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
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
