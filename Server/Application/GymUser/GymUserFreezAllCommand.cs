using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserFreezAllCommand : IRequest<GymUserResult> {}

    public class GymUserFreezAllCommandHandler : IRequestHandler<GymUserFreezAllCommand, GymUserResult>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserFreezAllCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<GymUserResult> Handle(GymUserFreezAllCommand request, CancellationToken cancellationToken)
        {
            var gymUserResult = await _gymUserService.FreezAllMemberships();

            return gymUserResult;
        }

    }
}
