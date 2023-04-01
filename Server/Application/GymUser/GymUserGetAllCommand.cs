using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserGetAllCommand : IRequest<IList<GymUserGetResult>> {}

    public class GymUserGetAllCommandHandler : IRequestHandler<GymUserGetAllCommand, IList<GymUserGetResult>>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserGetAllCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<IList<GymUserGetResult>> Handle(GymUserGetAllCommand request, CancellationToken cancellationToken)
        {
            var gymUsersResult = await _gymUserService.GetAll();
            return gymUsersResult;
        }

    }
}
