using FortuneTeller.Domain.Entities;

namespace FortuneTeller.Application.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}
