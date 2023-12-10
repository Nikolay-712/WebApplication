using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApp.Common.Exceptions.ClientSide;
using WebApp.Common.Exceptions.ServerSide;
using WebApp.Common.Resources;
using WebApp.Data;
using WebApp.Data.Entities;
using WebApp.Models.MappingExtensions;
using WebApp.Models.Request.Roles;
using WebApp.Models.Response.Roles;
using WebApp.Services.Extensions;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Implementations;

public class RoleService : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserService _userService;
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<RoleService> _logger;

    public RoleService(RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        IUserService userService,
        ApplicationContext applicationContext,
        ILogger<RoleService> logger)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _userService = userService;
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public async Task CreateAsync(CreateRoleRequestModel requestModel)
    {
        bool existsRole = await _roleManager.RoleExistsAsync(requestModel.Name);
        if (existsRole)
        {
            _logger.LogError("Role with name {roleName} already exists", requestModel.Name);
            throw new ExistsRoleException(Messages.RoleNameAlreadyExists);
        }

        ApplicationRole role = new()
        {
            Name = requestModel.Name,
            DescriptionEn = requestModel.DescriptionEn,
            DescriptionBg = requestModel.DescriptionBg,
        };

        IdentityResult identityResult = await _roleManager.CreateAsync(role);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new InvalidIdentityResultException(Messages.GeneralErrorMessage);
        }

        _logger.LogInformation("Succeeded create new role with name: {roleName}", requestModel.Name);
    }

    public async Task<RoleResponseModel> GetByIdAsync(Guid id)
    {
        ApplicationRole role = await FindByIdAsync(id);
        RoleResponseModel roleResponse = role.ToRoleResponseModel();
        return roleResponse;
    }

    public async Task<IReadOnlyList<RoleResponseModel>> GetAllAsync()
    {
        IReadOnlyList<RoleResponseModel> roles = await _roleManager.Roles
            .Select(x => x.ToRoleResponseModel())
            .ToListAsync();

        return roles;
    }

    public async Task UpdateAsync(Guid id, UpdateRoleRequestModel requestModel)
    {
        ApplicationRole role = await FindByIdAsync(id);

        bool existsName = await _applicationContext.Roles
            .AnyAsync(x => x.NormalizedName == requestModel.Name.ToUpper() && x.Id != role.Id);
        if (existsName)
        {
            _logger.LogError("Role with name {roleName} already exists", requestModel.Name);
            throw new ExistsRoleException(Messages.RoleNameAlreadyExists);
        }

        role.Name = requestModel.Name;
        role.DescriptionEn = requestModel.DescriptionEn;
        role.DescriptionBg = requestModel.DescriptionBg;


        IdentityResult identityResult = await _roleManager.UpdateAsync(role);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new InvalidIdentityResultException(Messages.GeneralErrorMessage);
        }

        _logger.LogInformation("Succeeded update role with ID: {roleId}", id);
    }

    public async Task RemoveAsync(Guid id)
    {
        ApplicationRole role = await FindByIdAsync(id);
        bool roleIsAssignedToUser = await _applicationContext.UserRoles.AnyAsync(x => x.RoleId == id);
        if (roleIsAssignedToUser)
        {
            _logger.LogError("Role with ID : {roleId} currently in use", id);
            throw new RemoveRoleException(Messages.RoleCurrentlyUse);
        }

        IdentityResult identityResult = await _roleManager.DeleteAsync(role);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new InvalidIdentityResultException(Messages.GeneralErrorMessage);
        }
    }

    public async Task AssignUserToRoleAsync(AssignToRoleRequestModel requestModel)
    {
        ApplicationUser user = await _userService.GetByIdAsync(requestModel.UserId);
        ApplicationRole role = await FindByIdAsync(requestModel.RoleId);

        bool isInRole = await _userManager.IsInRoleAsync(user, role.Name!);
        if (isInRole)
        {
            _logger.LogError("User with ID : {userId} is in role with ID : {roleId}", requestModel.UserId, requestModel.RoleId);
            throw new UserRoleExistsException(Messages.UserRoleExists);
        }

        IdentityResult identityResult = await _userManager.AddToRoleAsync(user, role.Name!);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new InvalidIdentityResultException(Messages.GeneralErrorMessage);
        }

        _logger.LogInformation("Succeeded assign user with ID : {userId} to role with ID : {roleId}", requestModel.UserId, requestModel.RoleId);
    }

    public async Task RemoveUserFromRoleAsync(RemoveFromRoleRequestModel requestModel)
    {
        ApplicationUser user = await _userService.GetByIdAsync(requestModel.UserId);
        ApplicationRole role = await FindByIdAsync(requestModel.RoleId);

        bool isInRole = await _userManager.IsInRoleAsync(user, role.Name!);
        if (!isInRole)
        {
            _logger.LogError("User with ID : {userId} is not in role with ID : {roleId}", requestModel.UserId, requestModel.RoleId);
            throw new UserNotInRoleException(Messages.UserNotInRole);
        }

        IdentityResult identityResult = await _userManager.RemoveFromRoleAsync(user, role.Name!);
        if (!identityResult.Succeeded)
        {
            _logger.LogError(identityResult.DisplayIdentityResultErrorMessages());
            throw new InvalidIdentityResultException(Messages.GeneralErrorMessage);
        }

        _logger.LogInformation("Succeeded remove user with ID : {userId} from role with ID : {roleId}", requestModel.UserId, requestModel.RoleId);
    }

    private async Task<ApplicationRole> FindByIdAsync(Guid id)
    {
        ApplicationRole? role = await _roleManager.FindByIdAsync(id.ToString());
        if (role is null)
        {
            _logger.LogError("Not found role with present ID : {roleId}", id);
            throw new NotFoundRoleException(Messages.RoleNotFound);
        }
        return role;
    }
}
