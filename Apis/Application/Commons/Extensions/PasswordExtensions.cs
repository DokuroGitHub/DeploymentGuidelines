namespace Application.Commons.Extensions;

public static class PasswordExtensions
{
    public static string Hash(this string inputString)
        => BCrypt.Net.BCrypt.HashPassword(inputString, salt: BCrypt.Net.BCrypt.GenerateSalt(9));

    public static bool Verify(this string password, string hashPassword)
        => BCrypt.Net.BCrypt.Verify(password, hashPassword);
}
