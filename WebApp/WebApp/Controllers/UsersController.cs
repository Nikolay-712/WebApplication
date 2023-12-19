using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Models.Request.Users;
using WebApp.Models.Response.Users;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("all")]
    public async Task<ResponseContent<PaginationResponseModel<UserResponseModel>>> GetAllAsync([FromQuery] UsersFilter usersFilter)
    {
        PaginationResponseModel<UserResponseModel> allUsers = await _userService.GetAllUsersAsync(usersFilter);
        return new ResponseContent<PaginationResponseModel<UserResponseModel>>
        {
            Result = allUsers
        };
    }
}
