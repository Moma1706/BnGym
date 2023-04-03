using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Auth.Login;

public record LoginCommand() : IRequest<Result>
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService) => _identityService = identityService;

    public async Task<Result> Handle(LoginCommand request, CancellationToken cancellationToken) => await _identityService.Login(request.Email, request.Password);
}