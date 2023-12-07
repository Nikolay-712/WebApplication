using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using WebApp.Client.Services.Implementations;
using WebApp.Client.Services.Interfaces;
using WebApp.Models;
using static WebApp.Common.Constants;

namespace WebApp.Client.Services;

public class ClientAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ITokenService _tokenService;
    private readonly HttpClient _httpClient;
    private readonly ClaimsPrincipal _anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());

    public ClientAuthenticationStateProvider(ITokenService tokenService, HttpClient httpClient)
    {
        _tokenService = tokenService;
        _httpClient = httpClient;
    }

    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string token = await _tokenService.GetAsync(TokenKey);
        if (string.IsNullOrEmpty(token))
        {
            return new AuthenticationState(_anonymousUser);
        }

        HttpResponseMessage responseMessage = await _httpClient.GetAsync($"{_httpClient.BaseAddress!.AbsoluteUri}api/authenticationState/validate/{token}");
        var response = await responseMessage.Content.ReadFromJsonAsync<ResponseContent<bool>>();

        if (!response!.Result)
        {
            await _tokenService.RemoveAsync(TokenKey);
            MarkUserAsLoggedOut();
            return new AuthenticationState(_anonymousUser);
        }

        IEnumerable<Claim> claims = _tokenService.ReadToken(token);
        if (claims is null)
        {
            return new AuthenticationState(_anonymousUser);
        }

        ClaimsPrincipal authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(claims, "apiauth"));
        Task<AuthenticationState> authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        _httpClient.AddJwtToken(token);

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