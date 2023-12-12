using System.Net.Http.Json;
using WebApp.Client.Services.Interfaces;
using WebApp.Models;
using WebApp.Models.Request.Roles;
using WebApp.Models.Response.Roles;

namespace WebApp.Client.Services.Implementations;

public class RoleClientService : IRoleClientService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public RoleClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = $"{httpClient.BaseAddress!.AbsoluteUri}api/roles/";
    }

    public async Task<ResponseContent> CreateAsync(CreateRoleRequestModel requestModel)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Post;
        requestMessage.RequestUri = new Uri($"{_baseUrl}create");
        requestMessage.Content = HttpClientHelper.GenerateRequestContent(requestModel);

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent<bool>>();
        return responseContent!;
    }

    public async Task<ResponseContent<IReadOnlyList<RoleResponseModel>>> GetAllAsync()
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Get;
        requestMessage.RequestUri = new Uri($"{_baseUrl}all");

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent<IReadOnlyList<RoleResponseModel>>? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent<IReadOnlyList<RoleResponseModel>>>();
        return responseContent!;
    }

    public async Task<ResponseContent<RoleResponseModel>> GetDetailsAsync(Guid id)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Get;
        requestMessage.RequestUri = new Uri($"{_baseUrl}{id}");

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent<RoleResponseModel>? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent<RoleResponseModel>>();
        return responseContent!;
    }

    public async Task<ResponseContent> RemoveAsync(Guid id)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Delete;
        requestMessage.RequestUri = new Uri($"{_baseUrl}remove/{id}");

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent? responseContent = await responseMessage.Content.ReadFromJsonAsync<ResponseContent<bool>>();
        return responseContent!;
    }
}
