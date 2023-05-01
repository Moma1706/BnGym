using System;
using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using MediatR;

namespace Application.Auth
{

    public record ChangePasswordCommand() : IRequest<Result>
    {
        public int Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IIdentityService _identityService;

        public ChangePasswordCommandHandler(IIdentityService identityService) => _identityService = identityService;

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken) => await _identityService.ChangePassword(request.Id, request.CurrentPassword, request.NewPassword);
    }
}

