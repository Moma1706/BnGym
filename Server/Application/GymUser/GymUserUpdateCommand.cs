using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
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

            public UpdateCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

            public async Task<GymUserResult> Handle(GymUserUpdateCommand data, CancellationToken cancellationToken)
            {
                var gymUserResult = await _gymUserService.Update(data.Id, data.Data);

                return gymUserResult;
            }
        }
    }
}

