using System.Security.Claims;
using Application.Commons.Models;

namespace Application.Interfaces;

public interface IJwtService
{
    string GenerateJWT(CurrentUser user);
    ClaimsPrincipal Validate(string token);
}
