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
        return Ok(new { message = "שלחנו הוראות לאיפוס הסיסמה לכתובת האימייל שלך." });
    }

    // POST /api/auth/reset-password
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken ct = default)
    {
        var result = await authService.ResetPasswordAsync(request, ct);
        return Ok(result);
    }

    // POST /api/auth/resend-verification (public — unverified users have no JWT)
    [HttpPost("resend-verification")]
    public async Task<IActionResult> ResendVerification([FromBody] ResendVerificationRequest request, CancellationToken ct = default)
    {
        await authService.ResendVerificationAsync(request.Email, ct);
        // Always return the same message — no info leak
        return Ok(new { message = "אם החשבון קיים וממתין לאימות, שלחנו אימייל חדש." });
    }
}
