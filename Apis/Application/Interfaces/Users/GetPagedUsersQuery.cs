namespace Application.Interfaces.Users;

public record GetPagedUsersQuery
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}