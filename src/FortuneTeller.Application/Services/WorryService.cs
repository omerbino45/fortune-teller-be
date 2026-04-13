using AutoMapper;
using FortuneTeller.Application.DTOs;
using FortuneTeller.Application.Exceptions;
using FortuneTeller.Application.Interfaces;
using FortuneTeller.Domain.Entities;
using FortuneTeller.Domain.Enums;
using FortuneTeller.Domain.Interfaces;

namespace FortuneTeller.Application.Services;

public class WorryService(IWorryRepository repository, IMapper mapper) : IWorryService
{
    public async Task<IReadOnlyList<WorryResponse>> GetAllAsync(Guid userId, CancellationToken ct = default)
    {
        var worries = await repository.GetAllAsync(userId, ct);
        return mapper.Map<IReadOnlyList<WorryResponse>>(worries);
    }

    public async Task<WorryResponse> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var worry = await repository.GetByIdAsync(id, userId, ct)
            ?? throw new NotFoundException($"Worry {id} not found.");

        return mapper.Map<WorryResponse>(worry);
    }

    public async Task<WorryResponse> CreateAsync(CreateWorryRequest request, Guid userId, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var worry = new Worry
        {
            Id                 = Guid.NewGuid(),
            Status             = WorryStatus.Active,
            Title              = request.Title,
            Description        = request.Description,
            Factors            = request.Factors,
            PreAnxietyLevel    = request.PreAnxietyLevel,
            Prophecy           = request.Prophecy,
            Assurance          = request.Assurance,
            CreatedAt          = now,
            AssuranceUpdatedAt = now,
            UserId             = userId
        };

        await repository.AddAsync(worry, ct);
        await repository.SaveChangesAsync(ct);

        return mapper.Map<WorryResponse>(worry);
    }

    public async Task<WorryResponse> PatchAsync(Guid id, PatchWorryRequest request, Guid userId, CancellationToken ct = default)
    {
        var worry = await repository.GetByIdAsync(id, userId, ct)
            ?? throw new NotFoundException($"Worry {id} not found.");

        if (request.Title is not null)
            worry.Title = request.Title;

        if (request.Description is not null)
            worry.Description = request.Description;

        if (request.Factors is not null)
            worry.Factors = request.Factors;

        if (request.IsArchived is not null)
            worry.IsArchived = request.IsArchived.Value;

        if (request.Assurance is not null && request.Assurance.Value != worry.Assurance)
        {
            worry.Assurance          = request.Assurance.Value;
            worry.AssuranceUpdatedAt = DateTime.UtcNow;
        }

        bool hasOutcome   = request.ActualOutcome is not null;
        bool hasPostLevel = request.PostAnxietyLevel is not null;

        if (hasOutcome && !hasPostLevel)
            throw new AppValidationException("PostAnxietyLevel is required when providing ActualOutcome.");

        if (!hasOutcome && hasPostLevel)
            throw new AppValidationException("ActualOutcome is required when providing PostAnxietyLevel.");

        if (hasOutcome && hasPostLevel)
        {
            if (worry.Status == WorryStatus.Active)
                worry.Status = WorryStatus.Resolved;

            worry.ActualOutcome    = request.ActualOutcome;
            worry.PostAnxietyLevel = request.PostAnxietyLevel;
        }

        await repository.SaveChangesAsync(ct);

        return mapper.Map<WorryResponse>(worry);
    }

    public async Task DeleteAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        var worry = await repository.GetByIdAsync(id, userId, ct)
            ?? throw new NotFoundException($"Worry {id} not found.");

        await repository.DeleteAsync(worry, ct);
        await repository.SaveChangesAsync(ct);
    }
}
