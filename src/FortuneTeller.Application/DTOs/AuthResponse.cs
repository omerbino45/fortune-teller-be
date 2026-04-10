namespace FortuneTeller.Application.DTOs;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsEmailVerified { get; set; }
}
