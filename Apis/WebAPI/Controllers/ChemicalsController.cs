using Application.DTO.Chemicals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Application.Interfaces;
using Application.Interfaces.Chemicals;

namespace WebAPI.Controllers;

public class ChemicalsController : ApiControllerBase
{
    private readonly IChemicalService _chemicalService;

    public ChemicalsController(IChemicalService chemicalService)
    {
        _chemicalService = chemicalService;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    => Ok(await _chemicalService.GetAllAsync());

    [HttpGet]
    public async Task<IActionResult> GetPagedListChemicals([FromQuery] GetPagedChemicalsQuery query)
    {
        var result = await _chemicalService.GetPagedListAsync(query);
        Response.Headers.Append("X-Pagination", result.ToString());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByKey(Guid id)
    => Ok(await _chemicalService.GetOneAsync(id));

    [HttpPost]
    public async Task<IActionResult> CreateChemical(ChemicalCreateDTO chemical)
    => Ok(await _chemicalService.CreateAsync(chemical));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, UpdateChemicalCommand request)
    {
        request.Id = id;

        await _chemicalService.UpdateAsync(request);

        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] JsonPatchDocument<UpdateChemicalCommand> patchDocument)
    {
        var command = new UpdateChemicalCommand();
        patchDocument.ApplyTo(command);
        command.Id = id;

        await _chemicalService.UpdateAsync(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _chemicalService.DeleteAsync(id);

        return NoContent();
    }
}
