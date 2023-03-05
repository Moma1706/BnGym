using Application.Common.Interfaces;
using Application.Enums;
using MediatR;

namespace Application.Auth.ForgotPassword;

public record ForgotPasswordCommand : IRequest
{
    public string Email { get; set; }
}

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand>
{
    private readonly IIdentityService _identityService;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(IIdentityService identityService, IEmailService emailService)
    {
        _identityService = identityService;
        _emailService = emailService;
    }

    public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var tokenResult = await _identityService.GenerateTokenForIdentityPurpose(request.Email, TokenPurpose.ResetPassword);

        if (tokenResult.Success)
            await _emailService.SendResetPasswordAsync(request.Email, tokenResult.Token);

        return Unit.Value;
    }
}