using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using WebApp.Common.Exceptions.ClientSide;
using WebApp.Common.Resources;
using WebApp.Data.Entities;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserService> _logger;

    public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ApplicationUser> GetByIdAsync(Guid userId)
    {
        ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            _logger.LogError("Not found user with ID: {userId}", userId);
            throw new NotFoundUserException(Messages.UserNotFound);
        }

        return user;
    }
}
