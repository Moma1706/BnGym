using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserExtendCommand : IRequest<GymUserResult>
    {
        public Guid Id { get; set; }
        public GymUserType Type { get; set; }
    }

    public class GymUserExtendCommandHandler : IRequestHandler<GymUserExtendCommand, GymUserResult>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserExtendCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<GymUserResult> Handle(GymUserExtendCommand request, CancellationToken cancellationToken)
        {
            var gymUserResult = await _gymUserService.ExtendMembership(request.Id, request.Type);

            return gymUserResult;
        }

    }
}
