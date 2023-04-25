using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Enums;
using MediatR;

namespace Application.Auth.Register;

public record RegisterCommand() : IRequest<RegisterResult>
{
    public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    public RegisterCommandHandler(IIdentityService identityService, IEmailService emailService)
    {
        _identityService = identityService;
        _emailService = emailService;
    }

    public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var registerResult = await _identityService.Register(request.Email, request.Password, request.FirstName, request.LastName, request.Address);

        if (registerResult.Success)
        {
            var tokenResult = await _identityService.GenerateTokenForIdentityPurpose(request.Email, TokenPurpose.ConfirmEmail);

            if (tokenResult.Success)
                await _emailService.SendConfirmationEmailAsync(request.Email, tokenResult.Token);
        }

        return registerResult;
    }
}