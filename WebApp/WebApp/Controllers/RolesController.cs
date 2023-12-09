﻿using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.Models.Request.Roles;
using WebApp.Models.Response.Roles;
using WebApp.Services.Interfaces;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost("create")]
    public async Task<ResponseContent> CreateAsync([FromBody] CreateRoleRequestModel requestModel)
    {
        await _roleService.CreateAsync(requestModel);
        return new ResponseContent();
    }

    [HttpGet("{id}")]
    public async Task<ResponseContent<RoleResponseModel>> GetByIdAsync([FromRoute] Guid id)
    {
        RoleResponseModel roleResponse = await _roleService.GetByIdAsync(id);
        return new ResponseContent<RoleResponseModel>()
        {
            Result = roleResponse,
        };
    }

    [HttpGet("all")]
    public async Task<ResponseContent<IReadOnlyList<RoleResponseModel>>> GetAllAsync()
    {
        IReadOnlyList<RoleResponseModel> roles = await _roleService.GetAllAsync();
        return new ResponseContent<IReadOnlyList<RoleResponseModel>>
        {
            Result = roles
        };
    }

    [HttpPut("update/{id}")]
    public async Task<ResponseContent> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateRoleRequestModel requestModel)
    {
        await _roleService.UpdateAsync(id, requestModel);
        return new ResponseContent();
    }

    [HttpDelete("remove/{id}")]
    public async Task<ResponseContent> RemoveAsync([FromRoute] Guid id)
    {
        await _roleService.RemoveAsync(id);
        return new ResponseContent();
    }
}
