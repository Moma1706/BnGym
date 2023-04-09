using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyTraining;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;

namespace Application.DailyTraining
{
    public record DailyTrainingGetAllCommand : IRequest<PageResult<DailyUsersGetResult>>
    {
        public string SearchString { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class DailyTrainingGetAllCommandHandler : IRequestHandler<DailyTrainingGetAllCommand, PageResult<DailyUsersGetResult>>
    {
        private readonly IDailyTrainingService _dailyTrainingService;

        public DailyTrainingGetAllCommandHandler(IDailyTrainingService dailyTrainingService) => _dailyTrainingService = dailyTrainingService;

        public async Task<PageResult<DailyUsersGetResult>> Handle(DailyTrainingGetAllCommand request, CancellationToken cancellationToken)
        {
            var result = await _dailyTrainingService.GetDailyUsers(request.SearchString, request.Page, request.PageSize);
            return result;
        }

    }
}
