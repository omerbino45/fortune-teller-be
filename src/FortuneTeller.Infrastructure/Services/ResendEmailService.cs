using FortuneTeller.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Resend;

namespace FortuneTeller.Infrastructure.Services;

public class ResendEmailService(IResend resend, IConfiguration configuration) : IEmailService
{
    private readonly string _fromAddress = configuration["Email:FromAddress"] ?? "onboarding@resend.dev";
    private readonly string _frontendBaseUrl = configuration["Email:FrontendBaseUrl"] ?? "http://localhost:5173";

    public async Task SendVerificationEmailAsync(string toEmail, string name, string token)
    {
        var verifyUrl = $"{_frontendBaseUrl}/verify-email?token={token}";

        var msg = new EmailMessage
        {
            From = _fromAddress,
            Subject = "אמת את כתובת האימייל שלך — Fortune Teller"
        };
        msg.To.Add(toEmail);
        msg.HtmlBody = $"""
            <div dir="rtl" style="font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto; padding: 24px; color: #1f2937;">
              <div style="text-align: center; margin-bottom: 24px;">
                <span style="font-size: 48px;">🔮</span>
                <h1 style="color: #7C3AED; margin: 8px 0 4px;">Fortune Teller</h1>
              </div>
              <h2 style="color: #374151;">שלום, {name}!</h2>
              <p style="color: #6b7280; line-height: 1.6;">
                תודה שנרשמת. כדי להשלים את הרישום ולהיכנס לחשבונך, אנא אמת את כתובת האימייל שלך:
              </p>
              <div style="text-align: center; margin: 32px 0;">
                <a href="{verifyUrl}"
                   style="background-color: #7C3AED; color: white; padding: 14px 32px; border-radius: 12px; text-decoration: none; font-weight: bold; font-size: 16px; display: inline-block;">
                  אמת את האימייל
                </a>
              </div>
              <p style="color: #9ca3af; font-size: 13px; text-align: center;">
                הקישור תקף ל-24 שעות. אם לא נרשמת, ניתן להתעלם מהודעה זו.
              </p>
            </div>
            """;

        await resend.EmailSendAsync(msg);
    }

    public async Task SendPasswordResetEmailAsync(string toEmail, string name, string token)
    {
        var resetUrl = $"{_frontendBaseUrl}/reset-password?token={token}";

        var msg = new EmailMessage
        {
            From = _fromAddress,
            Subject = "איפוס סיסמה — Fortune Teller"
        };
        msg.To.Add(toEmail);
        msg.HtmlBody = $"""
            <div dir="rtl" style="font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto; padding: 24px; color: #1f2937;">
              <div style="text-align: center; margin-bottom: 24px;">
                <span style="font-size: 48px;">🔮</span>
                <h1 style="color: #7C3AED; margin: 8px 0 4px;">Fortune Teller</h1>
              </div>
              <h2 style="color: #374151;">שלום, {name}!</h2>
              <p style="color: #6b7280; line-height: 1.6;">
                קיבלנו בקשה לאיפוס הסיסמה שלך. לחץ על הכפתור למטה כדי להגדיר סיסמה חדשה:
              </p>
              <div style="text-align: center; margin: 32px 0;">
                <a href="{resetUrl}"
                   style="background-color: #7C3AED; color: white; padding: 14px 32px; border-radius: 12px; text-decoration: none; font-weight: bold; font-size: 16px; display: inline-block;">
                  אפס סיסמה
                </a>
              </div>
              <p style="color: #9ca3af; font-size: 13px; text-align: center;">
                הקישור תקף לשעה אחת. אם לא ביקשת לאפס את הסיסמה, ניתן להתעלם מהודעה זו.
              </p>
            </div>
            """;

        await resend.EmailSendAsync(msg);
    }
}
