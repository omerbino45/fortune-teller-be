using FortuneTeller.Domain.Entities;
using FortuneTeller.Domain.Interfaces;
using FortuneTeller.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FortuneTeller.Infrastructure.Repositories;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Users.FindAsync([id], ct);

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await context.Users
            .FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower(), ct);

    public async Task<User?> GetByEmailVerificationTokenAsync(string token, CancellationToken ct = default)
        => await context.Users
            .FirstOrDefaultAsync(u => u.EmailVerificationToken == token, ct);

    public async Task<User?> GetByPasswordResetTokenAsync(string token, CancellationToken ct = default)
        => await context.Users
            .FirstOrDefaultAsync(u => u.PasswordResetToken == token, ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => await context.Users.AnyAsync(u => u.Email != null && u.Email.ToLower() == email.ToLower(), ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        user.Email = user.Email.ToLower();
        await context.Users.AddAsync(user, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
