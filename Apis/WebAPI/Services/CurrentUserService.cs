using System.Security.Claims;
using Application.Interfaces;

namespace WebAPI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var authorization = _httpContextAccessor.HttpContext?.Request.Headers.Authorization;
            var idString = _httpContextAccessor.HttpContext?.User?.FindFirstValue("ID");
            var nameIdentifierString = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"authorization: {authorization}");
            Console.WriteLine($"idString: {idString}");
            Console.WriteLine($"nameIdentifierString: {nameIdentifierString}");
            return Guid.TryParse(idString, out Guid id) ?
                id : Guid.TryParse(nameIdentifierString, out Guid nameIdentifier) ?
                nameIdentifier : null;
        }
    }
}
