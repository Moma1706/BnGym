using System;
using Application.App.Dtos;
using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using Application.GymUser.Dtos;
using MediatR;

namespace Application.App
{
    public class RegularUserUpdateCommand : IRequest<GymUserResult>
    {
        public Guid Id { get; set; }
        public UpdateRegularUserDto Data { get; set; }

        public class UpdateCommandHandler : IRequestHandler<RegularUserUpdateCommand, GymUserResult>
        {
            private readonly IGymUserService _gymUserService;
            private readonly IIdentityService _identityService;

            public UpdateCommandHandler(IIdentityService identityService, IGymUserService gymUserService)
            {
                _identityService = identityService;
                _gymUserService = gymUserService;
            }

            public async Task<GymUserResult> Handle(RegularUserUpdateCommand request, CancellationToken cancellationToken)
            {
                var gymUserResult = await _gymUserService.UpdateRegularUser(request.Id, request.Data);
                if (gymUserResult.Error.Code != 0)
                    return gymUserResult;

                return gymUserResult;
            }
        }
    }
}

