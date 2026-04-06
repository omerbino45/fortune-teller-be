using FortuneTeller.Domain.Entities;
using FortuneTeller.Domain.Interfaces;
using FortuneTeller.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FortuneTeller.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
        => await context.Users
            .FirstOrDefaultAsync(u => u.Username == username.ToLower(), ct);

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken ct = default)
        => await context.Users.AnyAsync(u => u.Username == username.ToLower(), ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        user.Username = user.Username.ToLower();
        await context.Users.AddAsync(user, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
