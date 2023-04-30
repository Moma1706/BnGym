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

            public UpdateCommandHandler(IIdentityService identityService, IGymUserService gymUserService)
            {
                _identityService = identityService;
                _gymUserService = gymUserService;
            }

            public async Task<GymUserResult> Handle(GymUserUpdateCommand request, CancellationToken cancellationToken)
            {
                var gymUserResult = await _gymUserService.Update(request.Id, request.Data);
                if (gymUserResult.Error.Code != 0)
                    return gymUserResult;

                return gymUserResult;
            }
        }
    }
}
