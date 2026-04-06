using FortuneTeller.Domain.Entities;

namespace FortuneTeller.Domain.Interfaces;

public interface IWorryRepository
{
    Task<IReadOnlyList<Worry>> GetAllAsync(Guid userId, CancellationToken ct = default);
    Task<Worry?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task AddAsync(Worry worry, CancellationToken ct = default);
    Task DeleteAsync(Worry worry, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
