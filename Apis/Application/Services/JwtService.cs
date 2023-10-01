using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Commons.Extensions;
using Application.Commons.Models;
using Application.Interfaces;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class JwtService : IJwtService
{
    private readonly IDateTimeService _dateTimeService;
    private readonly Appsettings _appSettings;

    public JwtService(
        IDateTimeService dateTimeService,
        Appsettings appSettings)
    {
        _dateTimeService = dateTimeService;
        _appSettings = appSettings;
    }

    public string GenerateJWT(CurrentUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim("Id", user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("Username", user.Username??""),
            new Claim("Role", user.Role.ToStringValue()),
            new Claim(ClaimTypes.Role, user.Role.ToStringValue()),
        };
        var token = new JwtSecurityToken(
            claims: claims,
            expires: _dateTimeService.Now.AddDays(_appSettings.Jwt.ExpireDays),
            audience: _appSettings.Jwt.Audience,
            issuer: _appSettings.Jwt.Issuer,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal Validate(string token)
    {
        IdentityModelEventSource.ShowPII = true;
        TokenValidationParameters validationParameters = new()
        {
            ValidateLifetime = true,
            ValidAudience = _appSettings.Jwt.Audience,
            ValidIssuer = _appSettings.Jwt.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.Key))
        };

        var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

        return principal;
    }
}
