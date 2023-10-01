namespace Application.Commons.Models;

public class Appsettings
{
    public bool UseInMemoryDatabase { get; init; }
    public bool SeedOnInit { get; init; }
    public string DatabaseConnection { get; init; } = null!;
    public Jwt Jwt { get; init; } = new Jwt();
}

public class Jwt
{
    public int ExpireDays { get; init; }
    public string Key { get; init; } = "HEHE YOU MIGHT THINK THIS IS THE REAL KEY BUT IT'S NOT";
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}
