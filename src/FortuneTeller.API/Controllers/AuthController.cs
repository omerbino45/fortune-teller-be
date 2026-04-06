using FortuneTeller.Application.DTOs;
using FortuneTeller.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FortuneTeller.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    // POST /api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct = default)
    {
        var result = await authService.RegisterAsync(request, ct);
        return Ok(result);
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct = default)
    {
        var result = await authService.LoginAsync(request, ct);
        return Ok(result);
    }
}
