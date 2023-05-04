using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyTraining;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;
using Microsoft.Data.SqlClient;

namespace Application.DailyTraining
{
    public record DailyTrainingGetAllCommand : IRequest<PageResult<DailyTrainingGetResult>>
    {
        public string SearchString { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

    }

    public class DailyTrainingGetAllCommandHandler : IRequestHandler<DailyTrainingGetAllCommand, PageResult<DailyTrainingGetResult>>
    {
        private readonly IDailyTrainingService _dailyTrainingService;

        public DailyTrainingGetAllCommandHandler(IDailyTrainingService dailyTrainingService) => _dailyTrainingService = dailyTrainingService;

        public async Task<PageResult<DailyTrainingGetResult>> Handle(DailyTrainingGetAllCommand request, CancellationToken cancellationToken)
        {
            var result = await _dailyTrainingService.GetDailyUsers(request.SearchString, request.Page, request.PageSize, request.SortOrder);
            return result;
        }

    }
}
