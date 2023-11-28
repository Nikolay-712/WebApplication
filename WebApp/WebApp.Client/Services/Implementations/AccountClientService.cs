using System.Net.Http;
using System;
using System.Net.Http.Json;
using WebApp.Client.Services.Interfaces;
using WebApp.Models;
using WebApp.Models.Request;
using WebApp.Models.Response;

namespace WebApp.Client.Services.Implementations;

public class AccountClientService : IAccountClientService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public AccountClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
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

        ResponseContent<LoginResponseModel>? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent<LoginResponseModel>>();
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
