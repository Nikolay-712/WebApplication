using System.Net.Http.Json;
using WebApp.Client.Services.Interfaces;
using WebApp.Models;
using WebApp.Models.Request.Users;
using WebApp.Models.Response.Users;

namespace WebApp.Client.Services.Implementations;

public class UserClientService : IUserClientService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public UserClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = $"{httpClient.BaseAddress!.AbsoluteUri}api/users/";
    }

    public async Task<ResponseContent<PaginationResponseModel<UserResponseModel>>> GetAllAsync(UsersFilter usersFilter)
    {
        using HttpRequestMessage requestMessage = new();

        requestMessage.Method = HttpMethod.Get;
        requestMessage.RequestUri = new Uri($"{_baseUrl}all");

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent<PaginationResponseModel<UserResponseModel>>? responseContent = await responseMessage
            .Content.ReadFromJsonAsync<ResponseContent<PaginationResponseModel<UserResponseModel>>>();

        return responseContent!;
    }
}
