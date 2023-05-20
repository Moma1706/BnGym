using Application.Common.Interfaces;
using Application.Common.Models.DailyUser;
using Application.Enums;
using MediatR;

namespace Application.DailyUser
{
    public record DailyUserGetOneCommand : IRequest<DailyUserGetResult>
    {
        public Guid Id { get; set; }
    }

    public class DailyUserGetOneCommandHandler : IRequestHandler<DailyUserGetOneCommand, DailyUserGetResult>
    {
        private readonly IDailyUserService _dailyUserService;

        public DailyUserGetOneCommandHandler(IDailyUserService dailyUserService) => _dailyUserService = dailyUserService;

        public async Task<DailyUserGetResult> Handle(DailyUserGetOneCommand request, CancellationToken cancellationToken)
        {
            var result = await _dailyUserService.GetOne(request.Id);

            return result;
        }

    }
}
