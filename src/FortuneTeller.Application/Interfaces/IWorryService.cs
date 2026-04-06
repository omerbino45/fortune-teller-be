using FortuneTeller.Application.DTOs;

namespace FortuneTeller.Application.Interfaces;

public interface IWorryService
{
    Task<IReadOnlyList<WorryResponse>> GetAllAsync(CancellationToken ct = default);
    Task<WorryResponse> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<WorryResponse> CreateAsync(CreateWorryRequest request, CancellationToken ct = default);
    Task<WorryResponse> PatchAsync(Guid id, PatchWorryRequest request, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
