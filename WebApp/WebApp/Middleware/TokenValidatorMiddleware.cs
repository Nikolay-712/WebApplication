using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mime;
using System.Net;
using System.Text;
using WebApp.Common.Configurations;

namespace WebApp.Middleware;

public class TokenValidatorMiddleware
{
    private readonly RequestDelegate _requestDelegate;
    private readonly JwtTokenSettings _jwtConfiguration;
    private readonly ILogger<TokenValidatorMiddleware> _logger;

    public TokenValidatorMiddleware(
        RequestDelegate requestDelegate,
        IOptions<JwtTokenSettings> options,
        ILogger<TokenValidatorMiddleware> logger)
    {
        _requestDelegate = requestDelegate;
        _jwtConfiguration = options.Value;
        _logger = logger;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        IHeaderDictionary headers = httpContext.Request.Headers;
        string authorizationHeader = httpContext!.Request.Headers["Authorization"].ToString();

        if (!string.IsNullOrEmpty(authorizationHeader))
        {
            string[] token = authorizationHeader!.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            TokenValidationResult result = await tokenHandler.ValidateTokenAsync(token[1], new TokenValidationParameters
            {
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _jwtConfiguration.Issuer,
                ValidAudience = _jwtConfiguration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Key))
            });

            if (!result.IsValid)
            {
                httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

                return;
            }
        }

        await _requestDelegate(httpContext);
    }
}
