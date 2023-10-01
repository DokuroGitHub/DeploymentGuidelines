using Application.Commons.Models;
using Application.DTO.Users;
using Application.Interfaces.Users;

namespace Application.Services;

public interface IUserService
{
    Task<IReadOnlyCollection<UserDTO>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task<PagedList<UserDTO>> GetPagedListAsync(
        GetPagedUsersQuery request,
        CancellationToken cancellationToken = default);
    Task<UserDTO> GetOneAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    Task<UserDTO> UpdateAsync(
        UpdateUserCommand request,
        CancellationToken cancellationToken = default);
}
