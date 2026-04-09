using FortuneTeller.Application.DTOs;
using FortuneTeller.Application.Exceptions;
using FortuneTeller.Application.Interfaces;
using FortuneTeller.Domain.Entities;
using FortuneTeller.Domain.Interfaces;

namespace FortuneTeller.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IJwtService jwtService,
    IEmailService emailService) : IAuthService
{
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        if (await userRepository.UsernameExistsAsync(request.Username, ct))
            throw new AppValidationException($"שם המשתמש '{request.Username}' כבר תפוס.");

        if (await userRepository.EmailExistsAsync(request.Email, ct))
            throw new AppValidationException("כתובת האימייל כבר בשימוש.");

        var verificationToken = Guid.NewGuid().ToString("N");

        var user = new User
        {
            Id                           = Guid.NewGuid(),
            Username                     = request.Username,
            PasswordHash                 = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Name                         = request.Name,
            Email                        = request.Email.ToLower(),
            IsEmailVerified              = false,
            EmailVerificationToken       = verificationToken,
            EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24),
            CreatedAt                    = DateTime.UtcNow
        };

        await userRepository.AddAsync(user, ct);
        await userRepository.SaveChangesAsync(ct);

        // Best-effort: don't fail registration if email sending fails
        try
        {
            await emailService.SendVerificationEmailAsync(user.Email, user.Name, verificationToken);
        }
        catch
        {
            // Log silently — user is created, they can resend later
        }

        return new RegisterResponse
        {
            Message = "נשלח אימייל לאישור. בדוק את תיבת הדואר שלך.",
            Email   = user.Email
        };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username, ct);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new AppValidationException("שם משתמש או סיסמה שגויים.");

        if (!user.IsEmailVerified)
            throw new EmailNotVerifiedException("עליך לאמת את כתובת האימייל שלך לפני הכניסה.");

        return BuildAuthResponse(user);
    }

    public async Task<AuthResponse> VerifyEmailAsync(string token, CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailVerificationTokenAsync(token, ct);

        if (user is null)
            throw new AppValidationException("קישור לא תקין.");

        if (user.EmailVerificationTokenExpiry < DateTime.UtcNow)
            throw new AppValidationException("הקישור פג תוקף. בקש קישור חדש.");

        user.IsEmailVerified              = true;
        user.EmailVerificationToken       = null;
        user.EmailVerificationTokenExpiry = null;

        await userRepository.SaveChangesAsync(ct);

        return BuildAuthResponse(user);
    }

    public async Task ForgotPasswordAsync(string email, CancellationToken ct = default)
    {
        var user = await userRepository.GetByEmailAsync(email, ct);

        // Always succeed — prevent user enumeration
        if (user is null) return;

        var resetToken = Guid.NewGuid().ToString("N");
        user.PasswordResetToken       = resetToken;
        user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);

        await userRepository.SaveChangesAsync(ct);

        try
        {
            await emailService.SendPasswordResetEmailAsync(user.Email!, user.Name, resetToken);
        }
        catch
        {
            // Silently swallow — response is always the same
        }
    }

    public async Task<AuthResponse> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken ct = default)
    {
        var user = await userRepository.GetByPasswordResetTokenAsync(request.Token, ct);

        if (user is null)
            throw new AppValidationException("קישור לא תקין.");

        if (user.PasswordResetTokenExpiry < DateTime.UtcNow)
            throw new AppValidationException("הקישור פג תוקף. בקש קישור חדש.");

        user.PasswordHash         = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
        user.PasswordResetToken   = null;
        user.PasswordResetTokenExpiry = null;

        await userRepository.SaveChangesAsync(ct);

        return BuildAuthResponse(user);
    }

    public async Task ResendVerificationAsync(Guid userId, CancellationToken ct = default)
    {
        var user = await userRepository.GetByIdAsync(userId, ct);
        if (user is null) return;
        if (user.IsEmailVerified) return;

        // Rate-limit: only regenerate if the existing token is older than 2 minutes
        if (user.EmailVerificationTokenExpiry.HasValue &&
            user.EmailVerificationTokenExpiry.Value > DateTime.UtcNow.AddHours(24).AddMinutes(-2))
        {
            throw new AppValidationException("נא להמתין לפחות 2 דקות לפני שליחה מחדש.");
        }

        var newToken = Guid.NewGuid().ToString("N");
        user.EmailVerificationToken       = newToken;
        user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24);

        await userRepository.SaveChangesAsync(ct);

        try
        {
            await emailService.SendVerificationEmailAsync(user.Email!, user.Name, newToken);
        }
        catch
        {
            // Silently swallow
        }
    }

    private AuthResponse BuildAuthResponse(User user) => new()
    {
        Token           = jwtService.GenerateToken(user),
        UserId          = user.Id,
        Username        = user.Username,
        Name            = user.Name,
        IsEmailVerified = user.IsEmailVerified
    };
}
