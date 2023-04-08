using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Application.Common.Models.DailyTraining;
using Application.Common.Models.GymUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DailyTraining
{

    public record DailyTrainingGetByDateCommand : IRequest<PageResult<DailyTrainingGetResult>>
    {
        public DateTime DateTime { get; set; }

        public string SearchString { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class DailyTrainingGetByDateCommandHandler : IRequestHandler<DailyTrainingGetByDateCommand, PageResult<DailyTrainingGetResult>>
    {
        private readonly IDailyTrainingService _dailyTrainingService;

        public DailyTrainingGetByDateCommandHandler(IDailyTrainingService dailyTrainingService) => _dailyTrainingService = dailyTrainingService;

        public async Task<PageResult<DailyTrainingGetResult>> Handle(DailyTrainingGetByDateCommand request, CancellationToken cancellationToken)
        {
            var dailyTrainings = await _dailyTrainingService.GetDailyByDate(request.DateTime, request.SearchString, request.Page, request.PageSize);
            return dailyTrainings;
        }

    }

}
