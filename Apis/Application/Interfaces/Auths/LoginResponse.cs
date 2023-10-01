using Application.Commons.Models;

namespace Application.Interfaces.Auths;

public class LoginResponse
{
    public CurrentUser CurrentUser { get; init; } = null!;
    public string Token { get; init; } = null!;
}
