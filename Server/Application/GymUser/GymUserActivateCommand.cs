using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserActivateCommand : IRequest<GymUserResult>
    {
        public Guid Id { get; set; }
    }

    public class GymUserActivateCommandHandler : IRequestHandler<GymUserActivateCommand, GymUserResult>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserActivateCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<GymUserResult> Handle(GymUserActivateCommand request, CancellationToken cancellationToken)
        {
            var gymUserResult = await _gymUserService.ActivateMembership(request.Id);

            return gymUserResult;
        }

    }
}
