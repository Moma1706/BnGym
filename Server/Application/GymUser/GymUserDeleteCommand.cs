using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserDeleteCommand : IRequest<GymUserResult>
    {
        public Guid Id { get; set; }
    }

    public class GymUserDeleteCommandHandler : IRequestHandler<GymUserDeleteCommand, GymUserResult>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserDeleteCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<GymUserResult> Handle(GymUserDeleteCommand request, CancellationToken cancellationToken)
        {
            var gymUserResult = await _gymUserService.Delete(request.Id);

            return gymUserResult;
        }
    }
}
