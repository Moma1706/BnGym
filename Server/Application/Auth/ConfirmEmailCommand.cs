using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Auth;

public record ConfirmEmailCommand : IRequest<Result>
{
    public string Email { get; set; }
    public string Token { get; set; }
}

public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly IIdentityService _identityService;

    public ConfirmEmailCommandHandler(IIdentityService identityService) => _identityService = identityService;

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken) => await _identityService.ConfirmEmail(request.Email, request.Token);
}