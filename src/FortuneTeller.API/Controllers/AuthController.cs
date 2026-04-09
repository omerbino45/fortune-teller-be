using System.Security.Claims;
using FortuneTeller.Application.DTOs;
using FortuneTeller.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

    // GET /api/auth/verify-email?token=xxx
    [HttpGet("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token, CancellationToken ct = default)
    {
        var result = await authService.VerifyEmailAsync(token, ct);
        return Ok(result);
    }

    // POST /api/auth/forgot-password
    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken ct = default)
    {
        await authService.ForgotPasswordAsync(request.Email, ct);
        return Ok(new { message = "אם הכתובת קיימת במערכת, שלחנו הוראות לאיפוס הסיסמה." });
    }

    // POST /api/auth/reset-password
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct = default)
    {
        var result = await authService.ResetPasswordAsync(request, ct);
        return Ok(result);
    }

    // POST /api/auth/resend-verification (requires valid JWT — user logged in but unverified)
    [HttpPost("resend-verification")]
    [Authorize]
    public async Task<IActionResult> ResendVerification(CancellationToken ct = default)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub");

        if (!Guid.TryParse(userIdStr, out var userId))
            return Unauthorized();

        await authService.ResendVerificationAsync(userId, ct);
        return Ok(new { message = "אימייל אימות נשלח מחדש." });
    }
}
