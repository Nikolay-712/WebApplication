using System.Text;
using System.Text.Json;

namespace WebApp.Client.Services.Implementations;

public static class HttpClientHelper
{
    public static StringContent GenerateRequestContent<TRequest>(TRequest requestModel)
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string jsonString = JsonSerializer.Serialize(requestModel, options);
        return new StringContent(jsonString, Encoding.UTF8, "application/json");
    }
}
