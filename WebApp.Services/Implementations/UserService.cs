using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Common.Exceptions.ClientSide;
using WebApp.Common.Resources;
using WebApp.Data.Entities;
using WebApp.Models;
using WebApp.Models.MappingExtensions;
using WebApp.Models.Request.Users;
using WebApp.Models.Response.Users;
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

    public async Task<PaginationResponseModel<UserResponseModel>> GetAllUsersAsync(UsersFilter usersFilter)
    {
        IQueryable<ApplicationUser> usersQuery = _userManager.Users;
        usersQuery = await ApplyUserFilterAsync(usersQuery, usersFilter);


        int totalCount = await usersQuery.CountAsync();
        int pagesCount = (int)Math.Ceiling((double)totalCount / usersFilter.ItemsPerPage!.Value);

        usersQuery = usersQuery
            .Skip(usersFilter.SkipCount)
            .Take(usersFilter.ItemsPerPage!.Value);

        IReadOnlyList<UserResponseModel> usersResponses = await usersQuery
            .Select(x => x.ToUserResponseModel(_userManager.GetRolesAsync(x).Result))
            .ToListAsync();

        return new PaginationResponseModel<UserResponseModel>
        {
            Items = usersResponses,
            TotalItems = totalCount,
            PageNumber = usersFilter.PageNumber!.Value,
            ItemsPerPage = usersFilter.ItemsPerPage!.Value,
            PagesCount = pagesCount,
        };

        throw new NotImplementedException();
    }

    private async Task<IQueryable<ApplicationUser>> ApplyUserFilterAsync(IQueryable<ApplicationUser> users, UsersFilter usersFilter)
    {
        if (usersFilter.SearchTerm is not null)
        {
            users = users.Where(x => x.Email!.Contains(usersFilter.SearchTerm));
        }

        return users.OrderByDescending(x => x.CreatedOn);
    }
}
