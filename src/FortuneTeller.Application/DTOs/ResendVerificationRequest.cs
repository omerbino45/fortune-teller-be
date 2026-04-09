using System.ComponentModel.DataAnnotations;

namespace FortuneTeller.Application.DTOs;

public class ResendVerificationRequest
{
    [Required]
    public string Username { get; set; } = string.Empty;
}
