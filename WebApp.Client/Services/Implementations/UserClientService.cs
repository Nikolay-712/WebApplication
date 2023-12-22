using System.Net.Http.Json;
using System.Text;
using WebApp.Client.Services.Interfaces;
using WebApp.Models;
using WebApp.Models.Request.Users;
using WebApp.Models.Response.Users;

namespace WebApp.Client.Services.Implementations;

public class UserClientService : IUserClientService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;
    private readonly int DefaultItemsPerPage = 10;
    private readonly int DefaultPageNumber = 1;

    public UserClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = $"{httpClient.BaseAddress!.AbsoluteUri}api/users/";
    }

    public async Task<ResponseContent<PaginationResponseModel<UserResponseModel>>> GetAllAsync(UsersFilter usersFilter)
    {
        using HttpRequestMessage requestMessage = new();
        requestMessage.Method = HttpMethod.Get;

        string requestUrl = AddQueryParameters(usersFilter, $"{_baseUrl}all");
        requestMessage.RequestUri = new Uri(requestUrl);

        using HttpResponseMessage responseMessage = await _httpClient.SendAsync(requestMessage);

        ResponseContent<PaginationResponseModel<UserResponseModel>>? responseContent = await responseMessage
            .Content.ReadFromJsonAsync<ResponseContent<PaginationResponseModel<UserResponseModel>>>();

        return responseContent!;
    }

    private string AddQueryParameters(UsersFilter usersFilter, string requestUrl)
    {
        StringBuilder queryParameters = new();

        if (!string.IsNullOrEmpty(usersFilter.SearchTerm))
        {
            queryParameters.Append($"searchTerm={usersFilter.SearchTerm}&");
        }

        if (usersFilter.ItemsPerPage != DefaultItemsPerPage)
        {
            queryParameters.Append($"itemsPerPage={usersFilter.ItemsPerPage}&");
        }

        if (usersFilter.PageNumber != DefaultPageNumber)
        {
            queryParameters.Append($"pageNumber={usersFilter.PageNumber}&");
        }

        return $"{requestUrl}?{queryParameters}";
    }
}
