using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserGetOneCommand : IRequest<GymUserGetResult>
    {
        public Guid Id { get; set; }
    }

    public class GymUserGetOneCommandHandler : IRequestHandler<GymUserGetOneCommand, GymUserGetResult>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserGetOneCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<GymUserGetResult> Handle(GymUserGetOneCommand request, CancellationToken cancellationToken)
        {
            var gymUserResult = await _gymUserService.GetOne(request.Id);

            return gymUserResult;
        }

    }
}
