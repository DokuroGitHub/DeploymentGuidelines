using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Application.Services;
using Application.Interfaces.Users;

namespace WebAPI.Controllers;

public class UsersController : ApiControllerBase
{
    private readonly IUserService _chemicalService;

    public UsersController(IUserService chemicalService)
    {
        _chemicalService = chemicalService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    => Ok(await _chemicalService.GetAllAsync());

    [HttpGet]
    public async Task<IActionResult> GetPagedListUsers([FromQuery] GetPagedUsersQuery query)
    {
        var result = await _chemicalService.GetPagedListAsync(query);
        Response.Headers.Append("X-Pagination", result.ToString());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByKey(Guid id)
    => Ok(await _chemicalService.GetOneAsync(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateUserCommand request)
    {
        request.Id = id;

        await _chemicalService.UpdateAsync(request);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] JsonPatchDocument<UpdateUserCommand> patchDocument)
    {
        var command = new UpdateUserCommand();
        patchDocument.ApplyTo(command);
        command.Id = id;

        await _chemicalService.UpdateAsync(command);

        return NoContent();
    }
}
