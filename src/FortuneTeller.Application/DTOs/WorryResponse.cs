namespace FortuneTeller.Application.DTOs;

public class WorryResponse
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<string> Factors { get; set; } = [];
    public int PreAnxietyLevel { get; set; }
    public string Prophecy { get; set; } = string.Empty;
    public int Assurance { get; set; }
    public string? ActualOutcome { get; set; }
    public int? PostAnxietyLevel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime AssuranceUpdatedAt { get; set; }
}
