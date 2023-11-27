﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using WebApp.Common.Exceptions.ClientSide;
using WebApp.Common.Exceptions.ServerSide;
using WebApp.Common.Resources;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Models.Request;
using WebApp.Models.Response;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly ApplicationContext _applicationContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenManager _jwtTokenManager;
    private readonly IEmailSenderService _emailSenderService;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        ApplicationContext applicationContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenManager jwtTokenManager,
        IEmailSenderService emailSenderService,
        ILogger<AccountService> logger)
    {
        _applicationContext = applicationContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenManager = jwtTokenManager;
        _emailSenderService = emailSenderService;
        _logger = logger;
    }

    public async Task RegistrationAsync(RegistrationRequestModel requestModel)
    {
        bool existsEmail = await _applicationContext.Users.AnyAsync(x => x.Email! == requestModel.Email);
        if (existsEmail)
        {
            _logger.LogError("Email already exists");
            throw new EmailAlreadyExistsException(Messages.EmailAlreadyExists);
        }

        bool existsUsername = await _applicationContext.Users.AnyAsync(x => x.NormalizedUserName == requestModel.UserName.ToUpper());
        if (existsUsername)
        {
            _logger.LogError("Username already exists");
            throw new EmailAlreadyExistsException(Messages.UserNameAlreadyExists);
        }

        ApplicationUser user = new()
        {
            UserName = requestModel.UserName,
            Email = requestModel.Email,
        };

        IdentityResult identityResult = await _userManager.CreateAsync(user, requestModel.Password);
        if (!identityResult.Succeeded)
        {
            IList<string> messages = identityResult.Errors.Select(x => x.Description).ToList();
            foreach (var errorMessage in messages)
            {
                _logger.LogError(errorMessage);
            }

            throw new NotSuccessfulRegistrationException(Messages.GeneralErrorMessage);
        }

        Uri confirmationUri = await GenerateEmailConfirmationUri(user.Email);
        await _emailSenderService.SendEmailConfirmationAsync(user.Email, confirmationUri.AbsoluteUri);

        _logger.LogInformation("Succeeded registration with email address: {email}", requestModel.Email);
    }

    public async Task<LoginResponseModel> LoginAsync(LoginRequestModel requestModel)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(requestModel.Email);
        if (user is null)
        {
            _logger.LogError("Not found user with {email}", requestModel.Email);
            throw new InvalidCredentialsException(Messages.InvalidCredentials);
        }

        LoginResponseModel loginResponse = new();
        if (!user.EmailConfirmed)
        {
            loginResponse.IsConfirmedEmail = false;
            return loginResponse;
        }

        SignInResult signInResult = await _signInManager.CheckPasswordSignInAsync(user, requestModel.Password, lockoutOnFailure: false);
        if (!signInResult.Succeeded)
        {
            _logger.LogError("Invalid authentication attempt: Email {email}", requestModel.Email);
            throw new InvalidCredentialsException(Messages.InvalidCredentials);
        }

        string token = await _jwtTokenManager.GenerateJwtTokenAsync(user);

        loginResponse.AccsesToken = token;
        loginResponse.IsConfirmedEmail = true;
        loginResponse.RememberMe = requestModel.RememberMe;

        return loginResponse;
    }

    public async Task ConfirmEmailAsync(ConfirmEmailRequestModel requestModel)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(requestModel.Identifire);
        if (user is null)
        {
            _logger.LogError("Email confirmation request contains invalid user ID {userId}", requestModel.Identifire);
            throw new InvalidConfirmationException("Request contains invalid user");
        }

        if (user.EmailConfirmed)
        {
            return;
        }

        string confirmEmailToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(requestModel.Token));
        bool isValidToken = await _userManager
            .VerifyUserTokenAsync(user, _userManager.Options.Tokens.EmailConfirmationTokenProvider, "EmailConfirmation", confirmEmailToken);
        if (!isValidToken)
        {
            _logger.LogError("Email confirmation request contains invalid confirmation token");
            throw new InvalidConfirmationException("Request contains invalid token");
        }

        IdentityResult identityResult = await _userManager.ConfirmEmailAsync(user, confirmEmailToken);
        if (!identityResult.Succeeded)
        {
            IList<string> messages = identityResult.Errors.Select(x => x.Description).ToList();
            foreach (var errorMessage in messages)
            {
                _logger.LogError(errorMessage);
            }
            throw new InvalidConfirmationException("Not succeeded email confirmation");
        }
    }

    public async Task<Uri> GenerateEmailConfirmationUri(string email)
    {
        ApplicationUser? user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            _logger.LogError("Not found user with Email: {email}", email);
            throw new NotFoundUserException(Messages.UserNotFound);
        }

        string token = await _jwtTokenManager.GenerateConfirmEmailTokenAsync(email);
        string baseUrl = "https://localhost:7061";

        Uri confirmationLink = new Uri($@"{baseUrl}/confirm-email?identifier={user.Id}&token={token}", new UriCreationOptions());
        return confirmationLink;
    }

}
