using Application.Commons.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

// [Authorize(Policy = PolicyNames.AdminOnly)]
public class AppController : ApiControllerBase
{
    private readonly Appsettings _appsettings;

    public AppController(Appsettings appsettings)
    {
        _appsettings = appsettings;
    }

    // [HttpGet("appsettings")]
    // public IActionResult GetAppsettings()
    //     => Ok(_appsettings);
}
