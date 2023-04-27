using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymWorker;
using Application.Enums;
using Application.GymWorker.Dtos;
using MediatR;

namespace Application.GymWorker
{
	public class GymWorkerUpdateCommand : IRequest<GymWorkerResult>
    {
        public Guid Id { get; set; }
        public UpdateGymWorkerDto Data { get; set; }

        public class GymWorkerUpdateCommandHandler : IRequestHandler<GymWorkerUpdateCommand, GymWorkerResult>
        {
            private readonly IGymWorkerService _gymWorkerService;
            private readonly IIdentityService _identityService;
            private readonly IEmailService _emailService;

            public GymWorkerUpdateCommandHandler(IGymWorkerService gymWorkerService, IIdentityService identityService, IEmailService emailService)
            {
                _gymWorkerService = gymWorkerService;
                _identityService = identityService;
                _emailService = emailService;
            }

            public async Task<GymWorkerResult> Handle(GymWorkerUpdateCommand request, CancellationToken cancellationToken)
            {
                var gymWorkerResult = await _gymWorkerService.Update(request.Id, request.Data);

                var tokenResult = await _identityService.GenerateTokenForIdentityPurpose(request.Data.Email, TokenPurpose.ConfirmEmail);
                if (tokenResult.Success)
                    await _emailService.SendConfirmationEmailAsync(request.Data.Email, tokenResult.Token);

                return gymWorkerResult;
            }
        }
    }
}

