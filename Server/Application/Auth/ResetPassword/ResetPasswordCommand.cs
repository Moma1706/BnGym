using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Auth.ResetPassword;

public record ResetPasswordCommand : IRequest<Result>
{
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }
}

public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IIdentityService _identityService;

    public ResetPasswordCommandHandler(IIdentityService identityService) => _identityService = identityService;

    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken) => await _identityService.ResetPassword(request.Email, request.Token, request.Password);
}