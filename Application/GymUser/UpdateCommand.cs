using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
	public class UpdateCommand : IRequest<GymUserResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public GymUserType Type { get; set; }

        public class UpdateCommandHandler : IRequestHandler<UpdateCommand, GymUserResult>
        {
            private readonly IGymUserService _gymUserService;

            public UpdateCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

            public async Task<GymUserResult> Update(GymUserGetOneCommand idData, UpdateCommand data, CancellationToken cancellationToken)
            {
                var gymUserResult = await _gymUserService.Update(idData.Id, data);

                return gymUserResult;
            }

            public Task<GymUserResult> Handle(UpdateCommand request, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
        }
    }
}

