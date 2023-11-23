using WebApp.Models.Request;
using WebApp.Models.Response;

namespace WebApp.Services.Interfaces;

public interface IAccountService
{
    Task RegistrationAsync(RegistrationRequestModel requestModel);

    Task<LoginResponseModel> LoginAsync(LoginRequestModel requestModel);
}
