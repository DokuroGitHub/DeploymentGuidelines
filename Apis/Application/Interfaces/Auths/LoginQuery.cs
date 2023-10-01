namespace Application.Interfaces.Auths;

public record LoginQuery
{
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
    public string? PasswordConfirm { get; init; }
};
