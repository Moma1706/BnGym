using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using Application.GymUser.Dtos;
using MediatR;

namespace Application.GymUser
{
	public class GymUserUpdateCommand : IRequest<GymUserResult>
    {
        public Guid Id { get; set; }
        public UpdateGymUserDto Data { get; set; }

        public class UpdateCommandHandler : IRequestHandler<GymUserUpdateCommand, GymUserResult>
        {
            private readonly IGymUserService _gymUserService;
            private readonly IIdentityService _identityService;
            private readonly IEmailService _emailService;

            public UpdateCommandHandler(IIdentityService identityService, IEmailService emailService, IGymUserService gymUserService)
            {
                _identityService = identityService;
                _emailService = emailService;
                _gymUserService = gymUserService;
            }

            public async Task<GymUserResult> Handle(GymUserUpdateCommand request, CancellationToken cancellationToken)
            {
                var gymUserResult = await _gymUserService.Update(request.Id, request.Data);
                if (gymUserResult.Error.Code != 0)
                    return gymUserResult;
                
                var tokenResult = await _identityService.GenerateTokenForIdentityPurpose(request.Data.Email, TokenPurpose.ConfirmEmail);
                if (tokenResult.Success)
                    await _emailService.SendConfirmationEmailAsync(request.Data.Email, tokenResult.Token);

                return gymUserResult;
            }
        }
    }
}

