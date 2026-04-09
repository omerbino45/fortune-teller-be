namespace FortuneTeller.Application.Interfaces;

public interface IEmailService
{
    Task SendVerificationEmailAsync(string toEmail, string name, string token);
    Task SendPasswordResetEmailAsync(string toEmail, string name, string token);
}
