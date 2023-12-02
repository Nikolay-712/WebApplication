using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Models.Request;
using WebApp.Models.Response;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService, IEmailSenderService emailSenderService)
    {
        _accountService = accountService;
    }

    [HttpPost("registration")]
    public async Task<ResponseContent<bool>> RegistrationAsync([FromBody] RegistrationRequestModel requestModel)
    {
        bool isSucceeded = await _accountService.RegistrationAsync(requestModel);
        return new ResponseContent<bool>()
        {
            Result = isSucceeded
        };
    }

    [HttpPost("login")]
    public async Task<ResponseContent<LoginResponseModel>> LoginAsync([FromBody] LoginRequestModel requestModel)
    {
        LoginResponseModel loginResponse = await _accountService.LoginAsync(requestModel);
        return new ResponseContent<LoginResponseModel>()
        {
            Result = loginResponse
        };
    }

    [HttpPost("confirm-email")]
    public async Task<ResponseContent> ConfirmEmailAsync([FromBody] ConfirmEmailRequestModel requestModel)
    {
        await _accountService.ConfirmEmailAsync(requestModel);
        return new ResponseContent();
    }

    [HttpGet("resend-email/{email}")]
    public async Task<ResponseContent> ResendEmailConfirmationAsync([FromRoute] string email)
    {
        await _accountService.ResendEmailConfirmationAsync(email);
        return new ResponseContent();
    }

    [HttpPost("reset-password")]
    public async Task<ResponseContent> ResetPasswordAsync(ResetPasswordRequestModel requestModel)
    {
        await _accountService.ResetPasswordAsync(requestModel);
        return new ResponseContent();
    }

    [HttpPost("change-password")]
    public async Task<ResponseContent> ChangePasswordAsync(ChangePasswordRequestModel requestModel)
    {
        await _accountService.ChangePasswordAsync(requestModel);
        return new ResponseContent();
    }
}
