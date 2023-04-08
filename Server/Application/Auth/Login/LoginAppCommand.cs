using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Auth.Login;

public record LoginAppCommand() : IRequest<Result>
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class LoginAppCommandHandler : IRequestHandler<LoginAppCommand, Result>
{
    private readonly IIdentityService _identityService;

    public LoginAppCommandHandler(IIdentityService identityService) => _identityService = identityService;

    public async Task<Result> Handle(LoginAppCommand request, CancellationToken cancellationToken) => await _identityService.LoginApp(request.Email, request.Password);
}