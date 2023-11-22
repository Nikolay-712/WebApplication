using WebApp.Models.Request;

namespace WebApp.Services.Interfaces;

public interface IAccountService
{
    Task RegistrationAsync(RegistrationRequestModel requestModel);
}
