using System.ComponentModel.DataAnnotations;

namespace FortuneTeller.Application.DTOs;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
