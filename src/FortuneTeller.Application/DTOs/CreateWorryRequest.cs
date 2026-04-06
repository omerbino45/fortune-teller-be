using System.ComponentModel.DataAnnotations;

namespace FortuneTeller.Application.DTOs;

public class CreateWorryRequest
{
    [Required]
    [MaxLength(50)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
    public List<string> Factors { get; set; } = [];

    [Required]
    [Range(0, 100)]
    public int PreAnxietyLevel { get; set; }

    [Required]
    public string Prophecy { get; set; } = string.Empty;

    [Required]
    [Range(0, 100)]
    public int Assurance { get; set; }
}
