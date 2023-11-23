using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Common.Exceptions.ClientSide;
using WebApp.Common.Exceptions.ServerSide;
using WebApp.Common.Resources;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Models.Request;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Implementations;

public class AccountService : IAccountService
{
    private readonly ApplicationContext _applicationContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        ApplicationContext applicationContext,
        UserManager<ApplicationUser> userManager,
        ILogger<AccountService> logger)
    {
        _applicationContext = applicationContext;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task RegistrationAsync(RegistrationRequestModel requestModel)
    {
        await RegistrationRequestValidation(requestModel);

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

    private async Task RegistrationRequestValidation(RegistrationRequestModel requestModel)
    {
        bool existsEmail = await _applicationContext.Users.AnyAsync(x => x.Email! == requestModel.Email);
        if (existsEmail)
        {
            _logger.LogError("Email already exists");
            throw new EmailAlreadyExistsException(Messages.EmailAlreadyExists);
        }

        bool existsUsername = await _applicationContext.Users.AnyAsync(x => x.NormalizedEmail == requestModel.Email.ToUpper());
        if (existsUsername)
        {
            _logger.LogError("Username already exists");
            throw new EmailAlreadyExistsException(Messages.UserNameAlreadyExists);
        }
    }
}
