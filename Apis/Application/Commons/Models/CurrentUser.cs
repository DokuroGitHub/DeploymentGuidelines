using Domain.Enums;

namespace Application.Commons.Models;

public class CurrentUser
{
    public Guid Id { get; init; }
    public string? Username { get; init; }
    public DateTime DateOfBirth { get; set; }
    public UserRole Role { get; init; }
}
