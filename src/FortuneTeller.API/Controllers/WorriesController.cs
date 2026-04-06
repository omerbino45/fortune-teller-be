using FortuneTeller.Application.DTOs;
using FortuneTeller.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FortuneTeller.API.Controllers;

[ApiController]
[Route("api/worries")]
public class WorriesController(IWorryService worryService) : ControllerBase
{
    // GET /api/worries
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await worryService.GetAllAsync(ct);
        return Ok(result);
    }

    // GET /api/worries/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await worryService.GetByIdAsync(id, ct);
        return Ok(result);
    }

    // POST /api/worries
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorryRequest request, CancellationToken ct = default)
    {
        var result = await worryService.CreateAsync(request, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PATCH /api/worries/{id}
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] PatchWorryRequest request, CancellationToken ct = default)
    {
        var result = await worryService.PatchAsync(id, request, ct);
        return Ok(result);
    }

    // DELETE /api/worries/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await worryService.DeleteAsync(id, ct);
        return NoContent();
    }
}
