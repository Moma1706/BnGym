namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendConfirmationEmailAsync(string email);

    Task SendResetPasswordAsync(string email, string token);

    Task SendEmailSubscriptionexpires(string email, string token);
}