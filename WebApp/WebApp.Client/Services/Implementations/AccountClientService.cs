using System.Net.Http;
using System;
using System.Net.Http.Json;
using WebApp.Client.Services.Interfaces;
using WebApp.Models;
using WebApp.Models.Request;
using WebApp.Models.Response;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

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

        await _tokenService.SetAsync("jwt_token", responseContent!.Result.AccsesToken);
        ((ClientAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(requestModel.Email);
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", responseContent!.Result.AccsesToken);


        return responseContent!;
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

}
