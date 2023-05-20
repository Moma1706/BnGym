using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Common.Models.User;
using MediatR;

namespace Application.User
{
    public class RegularUserGetCommand : IRequest<GymUserGetResult>
    {
        public Guid Id { get; set; }
    }

    public class RegularUserGetCommandHandler : IRequestHandler<RegularUserGetCommand, GymUserGetResult>
    {
        private readonly IGymUserService _gymUserService;

        public RegularUserGetCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<GymUserGetResult> Handle(RegularUserGetCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _gymUserService.GetRegularOne(request.Id);

            return userResult;
        }

    }
}

