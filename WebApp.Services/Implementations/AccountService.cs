using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        ApplicationContext applicationContext,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenManager jwtTokenManager,
        ILogger<AccountService> logger)
    {
        _applicationContext = applicationContext;
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenManager = jwtTokenManager;
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
   
}
