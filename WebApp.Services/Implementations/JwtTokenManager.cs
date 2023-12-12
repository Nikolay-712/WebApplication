using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApp.Common.Configurations;
using WebApp.Common.Exceptions.ClientSide;
using WebApp.Data.Entities;
using WebApp.Services.Interfaces;

namespace WebApp.Services.Implementations;

public class JwtTokenManager : IJwtTokenManager
{
    private readonly JwtTokenSettings _jwtTokenSettings;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenManager(IOptions<JwtTokenSettings> options, UserManager<ApplicationUser> userManager)
    {
        _jwtTokenSettings = options.Value;
        _userManager = userManager;
    }

    public async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_jwtTokenSettings.Key);
        IList<string> roles = await _userManager.GetRolesAsync(user);


        IList<Claim> userClaims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        ];

        foreach (string role in roles)
        {
            userClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        JwtSecurityToken token = new(
            issuer: _jwtTokenSettings.Issuer,
            audience: _jwtTokenSettings.Audience,
            claims: userClaims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature));

        return tokenHandler.WriteToken(token);
    }

    public async Task<string> GenerateConfirmEmailTokenAsync(ApplicationUser user)
    {
        string confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        confirmEmailToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(confirmEmailToken));

        return confirmEmailToken;
    }

    public async Task<string> GenerateForgetPasswordTokenAsync(ApplicationUser user)
    {
        string resetPasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        resetPasswordToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetPasswordToken));

        return resetPasswordToken;
    }
}
