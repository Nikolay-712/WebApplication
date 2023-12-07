using System.Net.Http.Json;
using WebApp.Client.Services.Interfaces;
using WebApp.Models;
using WebApp.Models.Request;
using WebApp.Models.Response;
using Microsoft.AspNetCore.Components.Authorization;
using static WebApp.Common.Constants;

namespace WebApp.Client.Services.Implementations;

public class AccountClientService : IAccountClientService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenService _tokenService;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly string _baseUrl;

    public AccountClientService(HttpClient httpClient, ITokenService tokenService, AuthenticationStateProvider authenticationStateProvider)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _authenticationStateProvider = authenticationStateProvider;
        _baseUrl = $"{httpClient.BaseAddress!.AbsoluteUri}api/accounts/";
    }

    public async Task<ResponseContent<bool>> RegistrationAsync(RegistrationRequestModel requestModel)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Post;
        requestMessage.RequestUri = new Uri($"{_baseUrl}registration");
        requestMessage.Content = HttpClientHelper.GenerateRequestContent(requestModel);

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent<bool>? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent<bool>>();
        return responseContent!;
    }

    public async Task<ResponseContent<LoginResponseModel>> LoginAsync(LoginRequestModel requestModel)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Post;
        requestMessage.RequestUri = new Uri($"{_baseUrl}login");
        requestMessage.Content = HttpClientHelper.GenerateRequestContent(requestModel);

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent<LoginResponseModel>? responseContent = await responseMessage.Content
            .ReadFromJsonAsync<ResponseContent<LoginResponseModel>>();

        if (responseContent!.Result is not null)
        {
            string token = responseContent!.Result.AccsesToken;

            await _tokenService.SetAsync(TokenKey, token);
            ((ClientAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(token);
            _httpClient.AddJwtToken(token);
        }

        return responseContent!;
    }

    public async Task LogoutAsync()
    {
        await _tokenService.RemoveAsync(TokenKey);
        ((ClientAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.RemoveJwtToken();
    }

    public async Task<ResponseContent> ConfirmEmailAsync(ConfirmEmailRequestModel requestModel)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Post;
        requestMessage.RequestUri = new Uri($"{_baseUrl}confirm-email");
        requestMessage.Content = HttpClientHelper.GenerateRequestContent(requestModel);

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent>();
        return responseContent!;
    }

    public async Task<ResponseContent> ResendEmailConfirmationAsync(string email)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Get;
        requestMessage.RequestUri = new Uri($"{_baseUrl}resend-email/{email}");

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent>();
        return responseContent!;
    }

    public async Task<ResponseContent> ResetPasswordAsync(ResetPasswordRequestModel requestModel)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Post;
        requestMessage.RequestUri = new Uri($"{_baseUrl}reset-password");
        requestMessage.Content = HttpClientHelper.GenerateRequestContent(requestModel);

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent>();
        return responseContent!;
    }

    public async Task<ResponseContent> ChangePasswordAsync(ChangePasswordRequestModel requestModel)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Post;
        requestMessage.RequestUri = new Uri($"{_baseUrl}change-password");
        requestMessage.Content = HttpClientHelper.GenerateRequestContent(requestModel);

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent>();
        return responseContent!;
    }

    public async Task<ResponseContent<UserProfileResponseModel>> GetProfileAsync()
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Get;
        requestMessage.RequestUri = new Uri($"{_baseUrl}profile");

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent<UserProfileResponseModel>? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent<UserProfileResponseModel>>();
        return responseContent!;
    }

}
