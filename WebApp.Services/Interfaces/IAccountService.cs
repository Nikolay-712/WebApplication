using WebApp.Models.Request;
using WebApp.Models.Response;

namespace WebApp.Services.Interfaces;

public interface IAccountService
{
    Task<bool> RegistrationAsync(RegistrationRequestModel requestModel);

    Task<LoginResponseModel> LoginAsync(LoginRequestModel requestModel);

    Task ConfirmEmailAsync(ConfirmEmailRequestModel requestModel);

    Task ResendEmailConfirmationAsync(string email);

    Task ResetPasswordAsync(ResetPasswordRequestModel requestModel);

    Task ChangePasswordAsync(ChangePasswordRequestModel requestModel);
}
