using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using FortuneTeller.Application.DTOs;
using FortuneTeller.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FortuneTeller.API.Controllers;

[Authorize]
[ApiController]
[Route("api/worries")]
public class WorriesController(IWorryService worryService) : ControllerBase
{
    private Guid CurrentUserId =>
        Guid.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub)
            ?? throw new InvalidOperationException("UserId claim missing."));

    // GET /api/worries
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct = default)
    {
        var result = await worryService.GetAllAsync(CurrentUserId, ct);
        return Ok(result);
    }

    // GET /api/worries/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await worryService.GetByIdAsync(id, CurrentUserId, ct);
        return Ok(result);
    }

    // POST /api/worries
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWorryRequest request, CancellationToken ct = default)
    {
        var result = await worryService.CreateAsync(request, CurrentUserId, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PATCH /api/worries/{id}
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Patch(Guid id, [FromBody] PatchWorryRequest request, CancellationToken ct = default)
    {
        var result = await worryService.PatchAsync(id, request, CurrentUserId, ct);
        return Ok(result);
    }

    // DELETE /api/worries/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        await worryService.DeleteAsync(id, CurrentUserId, ct);
        return NoContent();
    }
}
