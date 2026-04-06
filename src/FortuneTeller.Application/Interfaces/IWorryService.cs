using FortuneTeller.Application.DTOs;

namespace FortuneTeller.Application.Interfaces;

public interface IWorryService
{
    Task<IReadOnlyList<WorryResponse>> GetAllAsync(Guid userId, CancellationToken ct = default);
    Task<WorryResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<WorryResponse> CreateAsync(CreateWorryRequest request, Guid userId, CancellationToken ct = default);
    Task<WorryResponse> PatchAsync(Guid id, PatchWorryRequest request, Guid userId, CancellationToken ct = default);
    Task DeleteAsync(Guid id, Guid userId, CancellationToken ct = default);
}
