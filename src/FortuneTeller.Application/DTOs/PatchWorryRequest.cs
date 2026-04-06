using System.ComponentModel.DataAnnotations;

namespace FortuneTeller.Application.DTOs;

public class PatchWorryRequest
{
    [MaxLength(50)]
    public string? Title { get; set; }

    public string? Description { get; set; }
    public List<string>? Factors { get; set; }

    [Range(0, 100)]
    public int? Assurance { get; set; }

    // Resolution fields — if both are provided and status is Active, triggers transition to Resolved
    public string? ActualOutcome { get; set; }

    [Range(0, 100)]
    public int? PostAnxietyLevel { get; set; }
}
