using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserCreateCommand : IRequest<GymUserResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool IsStudent { get; set; }
        public GymUserType Type { get; set; }
    }

    public class GymUserCreateCommandHandler : IRequestHandler<GymUserCreateCommand, GymUserResult>
    {
        private readonly IGymUserService _gymUserService;

        public GymUserCreateCommandHandler(IGymUserService gymUserService) => _gymUserService = gymUserService;

        public async Task<GymUserResult> Handle(GymUserCreateCommand request, CancellationToken cancellationToken)
        {
            var gymUserResult = await _gymUserService.Create(request.FirstName, request.LastName, request.Email, request.Address, request.IsStudent, request.Type);

            return gymUserResult;
        }

    }
}
