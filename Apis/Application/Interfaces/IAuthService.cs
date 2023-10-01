using Application.DTO.Users;
using Application.Interfaces.Auths;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<GetCurrentUserIdResponse> GetCurrentUserIdAsync();
    Task<UserDTO> GetCurrentUserAsync(
          CancellationToken cancellationToken = default);
    Task RegisterAsync(
       RegisterCommand request,
       CancellationToken cancellationToken = default);
    Task<LoginResponse> LoginAsync(
       LoginQuery request,
       CancellationToken cancellationToken = default);
}
