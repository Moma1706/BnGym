using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserFreezCommand : IRequest<GymUserResult>
    {
        public Guid Id { get; set; }
    }

    public class GymUserFreezCommandHandler : IRequestHandler<GymUserFreezCommand, GymUserResult>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserFreezCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<GymUserResult> Handle(GymUserFreezCommand request, CancellationToken cancellationToken)
        {
            var gymUserResult = await _gymUserService.FreezMembership(request.Id);

            return gymUserResult;
        }

    }
}
