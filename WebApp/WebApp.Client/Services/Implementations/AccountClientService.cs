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
    private readonly string _endpointAddress;

    public AccountClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = httpClient.BaseAddress!.AbsoluteUri;
        _endpointAddress = "api/accounts/";
    }

    public async Task<ResponseContent> RegistrationAsync(RegistrationRequestModel requestModel)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Post;
        requestMessage.RequestUri = new Uri($"{_baseUrl}{_endpointAddress}registration");
        requestMessage.Content = HttpClientHelper.GenerateRequestContent(requestModel);

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent>();
        return responseContent;
    }

    public async Task<ResponseContent<LoginResponseModel>> LoginAsync(LoginRequestModel requestModel)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Post;
        requestMessage.RequestUri = new Uri($"{_baseUrl}{_endpointAddress}login");
        requestMessage.Content = HttpClientHelper.GenerateRequestContent(requestModel);

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent<LoginResponseModel> responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent<LoginResponseModel>>();
        return responseContent;
    }

}
