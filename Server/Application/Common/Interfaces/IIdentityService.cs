using Application.Common.Models.Auth;
using Application.Enums;

namespace Application.Common.Interfaces;

public interface IIdentityService
{
    Task<RegisterResult> Register(string email, string password, string firstName, string lastName, string address);

    Task<Result> Login(string email, string password);
    Task<Result> LoginApp(string email, string password);

    Task<Result> ConfirmEmail(string email, string password);

    Task<Result> GenerateTokenForIdentityPurpose(string email, TokenPurpose purpose);

    Task<Result> ResetPassword(string email, string token, string password);

    Task<Result> ChangePassword(int id, string currentPassword, string newPassword);
}