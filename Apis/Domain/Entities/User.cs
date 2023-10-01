using Domain.Enums;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public UserRole Role { get; set; }
}
