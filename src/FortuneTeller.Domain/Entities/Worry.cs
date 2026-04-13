using FortuneTeller.Domain.Enums;

namespace FortuneTeller.Domain.Entities;

public class Worry
{
    public Guid Id { get; set; }
    public WorryStatus Status { get; set; } = WorryStatus.Active;
    public bool IsArchived { get; set; } = false;

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> Factors { get; set; } = [];

    // Locked after creation — enforced in service + EF configuration
    public int PreAnxietyLevel { get; set; }
    public string Prophecy { get; set; } = string.Empty;

    public int Assurance { get; set; }
    public string? ActualOutcome { get; set; }
    public int? PostAnxietyLevel { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime AssuranceUpdatedAt { get; set; }

    // Ownership
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}
