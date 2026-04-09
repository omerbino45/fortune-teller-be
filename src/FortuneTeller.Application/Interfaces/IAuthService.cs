using FortuneTeller.Application.DTOs;

namespace FortuneTeller.Application.Interfaces;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<AuthResponse> VerifyEmailAsync(string token, CancellationToken ct = default);
    Task ForgotPasswordAsync(string email, CancellationToken ct = default);
    Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default);
    Task ResendVerificationAsync(string username, CancellationToken ct = default);
}
