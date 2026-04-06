using FortuneTeller.Application.DTOs;
using FortuneTeller.Application.Exceptions;
using FortuneTeller.Application.Interfaces;
using FortuneTeller.Domain.Entities;
using FortuneTeller.Domain.Interfaces;

namespace FortuneTeller.Application.Services;

public class AuthService(IUserRepository userRepository, IJwtService jwtService) : IAuthService
{
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        if (await userRepository.UsernameExistsAsync(request.Username, ct))
            throw new AppValidationException($"Username '{request.Username}' is already taken.");

        var user = new User
        {
            Id           = Guid.NewGuid(),
            Username     = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Name         = request.Name,
            CreatedAt    = DateTime.UtcNow
        };

        await userRepository.AddAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);

        return new AuthResponse
        {
            Token    = jwtService.GenerateToken(user),
            UserId   = user.Id,
            Username = user.Username,
            Name     = user.Name
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, ct);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new AppValidationException("Invalid username or password.");

        return new AuthResponse
        {
            Token    = jwtService.GenerateToken(user),
            UserId   = user.Id,
            Username = user.Username,
            Name     = user.Name
        };
    }
}
