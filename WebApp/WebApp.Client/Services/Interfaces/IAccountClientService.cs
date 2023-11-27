using WebApp.Models.Request;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Client.Services.Interfaces;

public interface IAccountClientService
{
    Task<ResponseContent> RegistrationAsync(RegistrationRequestModel requestModel);

    Task<ResponseContent<LoginResponseModel>> LoginAsync(LoginRequestModel requestModel);

    Task<ResponseContent> ConfirmEmailAsync(ConfirmEmailRequestModel requestModel);
}
