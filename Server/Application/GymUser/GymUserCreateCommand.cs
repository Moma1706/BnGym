using Application.Common.Interfaces;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.GymUser
{
    public record GymUserCreateCommand : IRequest<GymUserGetResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public GymUserType Type { get; set; }
    }

    public class GymUserCreateCommandHandler : IRequestHandler<GymUserCreateCommand, GymUserGetResult>
    {
        private readonly IGymUserService _gymUserService;
        private readonly IIdentityService _identityService;
        private readonly IEmailService _emailService;


        public GymUserCreateCommandHandler(IIdentityService identityService, IEmailService emailService, IGymUserService gymUserService)
        {
            _identityService = identityService;
            _emailService = emailService;
            _gymUserService = gymUserService;
        }

        public async Task<GymUserGetResult> Handle(GymUserCreateCommand request, CancellationToken cancellationToken)
        {
            var gymUserResult = await _gymUserService.Create(request.FirstName, request.LastName, request.Email, request.Address, request.Type);
            if (gymUserResult.Error.Code != 0)
                return gymUserResult;
       
            _emailService.SendConfirmationEmailAsync(gymUserResult.Email);
            return gymUserResult;
        }
    }
}
