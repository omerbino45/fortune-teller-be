using FortuneTeller.Domain.Entities;
using FortuneTeller.Domain.Interfaces;
using FortuneTeller.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FortuneTeller.Infrastructure.Repositories;

public class WorryRepository(AppDbContext context) : IWorryRepository
{
    public async Task<IReadOnlyList<Worry>> GetAllAsync(CancellationToken ct = default)
        => await context.Worries
            .AsNoTracking()
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync(ct);

    public async Task<Worry?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await context.Worries.FindAsync([id], ct);

    public async Task AddAsync(Worry worry, CancellationToken ct = default)
        => await context.Worries.AddAsync(worry, ct);

    public Task DeleteAsync(Worry worry, CancellationToken ct = default)
    {
        context.Worries.Remove(worry);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
