using WebApp.Models.Request;
using WebApp.Models;
using WebApp.Models.Response;

namespace WebApp.Client.Services.Interfaces;

public interface IAccountClientService
{
    Task<ResponseContent<bool>> RegistrationAsync(RegistrationRequestModel requestModel);

    Task<ResponseContent<LoginResponseModel>> LoginAsync(LoginRequestModel requestModel);

    Task LogoutAsync();

    Task<ResponseContent> ConfirmEmailAsync(ConfirmEmailRequestModel requestModel);

    Task<ResponseContent> ResendEmailConfirmationAsync(string email);

    Task<ResponseContent> ResetPasswordAsync(ResetPasswordRequestModel requestModel);

    Task<ResponseContent> ChangePasswordAsync(ChangePasswordRequestModel requestModel);
}
