using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebApp.Common.Configurations;
using WebApp.Models;

namespace WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationStateController : ControllerBase
{
    private readonly JwtTokenSettings _jwtSettings;
    private readonly ILogger<AuthenticationStateController> _logger;

    public AuthenticationStateController(IOptions<JwtTokenSettings> options, ILogger<AuthenticationStateController> logger)
    {
        _jwtSettings = options.Value;
        _logger = logger;
    }

    [HttpGet("validate/{token}")]
    public async Task<ResponseContent<bool>> ValidateTokenAsync([FromRoute] string token)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        TokenValidationResult result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
        {
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key))
        });

        _logger.LogInformation("Token in local storage is with valid parameters {resultStat} message: {resultMessage}",
            result.IsValid,
            result.Exception is null ? string.Empty : result.Exception.Message);

        return new ResponseContent<bool>()
        {
            Result = result.IsValid,
        };
    }
}
