﻿using Microsoft.AspNetCore.Mvc;
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

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("registration")]
    public async Task<ResponseContent> RegistrationAsync([FromBody] RegistrationRequestModel requestModel)
    {
        await _accountService.RegistrationAsync(requestModel);
        return new ResponseContent();
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
}