using Domain.Enums;

namespace Application.Commons.Extensions;

public static class UserRoleExtensions
{
    public static string ToStringValue(this UserRole val)
    {
        switch (val)
        {
            case UserRole.Admin:
                return nameof(UserRole.Admin);
            case UserRole.User:
            default:
                return nameof(UserRole.User);
        }
    }

    public static string? ToStringValueOrDefault(this UserRole val)
    {
        switch (val)
        {
            case UserRole.Admin:
                return nameof(UserRole.Admin);
            case UserRole.User:
                return nameof(UserRole.User);
            default:
                return null;
        }
    }

    public static UserRole ToUserRole(this string? val)
    {
        switch (val)
        {
            case nameof(UserRole.Admin):
                return UserRole.Admin;
            case nameof(UserRole.User):
            default:
                return UserRole.User;
        }
    }

    public static UserRole? ToUserRoleOrDefault(this string? val)
    {
        switch (val)
        {
            case nameof(UserRole.Admin):
                return UserRole.Admin;
            case nameof(UserRole.User):
                return UserRole.User;
            default:
                return null;
        }
    }
}
