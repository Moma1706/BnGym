namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string email, string token);

    Task SendResetPasswordAsync(string email, string token);
}