using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserActivateAllCommand : IRequest<GymUserResult> { }

    public class GymUserActivateAllCommandHandler : IRequestHandler<GymUserActivateAllCommand, GymUserResult>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserActivateAllCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<GymUserResult> Handle(GymUserActivateAllCommand request, CancellationToken cancellationToken)
        {
            var gymUserResult = await _gymUserService.ActivateAllMemberships();

            return gymUserResult;
        }

    }
}
