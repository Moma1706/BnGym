using Application.Common.Interfaces;
using Application.Common.Models.DailyUser;
using Application.Enums;
using MediatR;

namespace Application.DailyUser
{
    public record DailyUserAddArrivalCommand : IRequest<DailyUserResult>
    {
        public Guid Id { get; set; }
    }

    public class GymUserGetOneCommandHandler : IRequestHandler<DailyUserAddArrivalCommand, DailyUserResult>
    {
        private readonly IDailyUserService _dailyUserService;

        public GymUserGetOneCommandHandler(IDailyUserService dailyTUserService) => _dailyUserService = dailyTUserService;

        public async Task<DailyUserResult> Handle(DailyUserAddArrivalCommand request, CancellationToken cancellationToken)
        {
            var result = await _dailyUserService.AddArrival(request.Id);

            return result;
        }

    }
}
