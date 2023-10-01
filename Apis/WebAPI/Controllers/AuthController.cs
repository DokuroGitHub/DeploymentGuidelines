using Application.Interfaces;
using Application.Interfaces.Auths;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

public class AuthController : ApiControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("[action]")]
    public async Task<ActionResult> CurrentUserId()
    => Ok(await _authService.GetCurrentUserIdAsync());

    [HttpGet("[action]")]
    public async Task<ActionResult> CurrentUser()
    => Ok(await _authService.GetCurrentUserAsync());

    [HttpPost("[action]")]
    public async Task<ActionResult> Register(RegisterCommand request)
    {
        await _authService.RegisterAsync(request);

        return NoContent();
    }

    [HttpPost("[action]")]
    public async Task<ActionResult> Login(LoginQuery request)
    => Ok(await _authService.LoginAsync(request));
}
