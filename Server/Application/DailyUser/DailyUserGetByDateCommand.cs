using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Application.Common.Models.DailyUser;
using Application.Common.Models.GymUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DailyUser
{
    public record DailyUserGetByDateCommand : IRequest<PageResult<DailyHistoryGetResult>>
    {
        public DateTime DateTime { get; set; }

        public string SearchString { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class DailyUserGetByDateCommandHandler : IRequestHandler<DailyUserGetByDateCommand, PageResult<DailyHistoryGetResult>>
    {
        private readonly IDailyUserService _dailyUserService;

        public DailyUserGetByDateCommandHandler(IDailyUserService dailyUserService) => _dailyUserService = dailyUserService;

        public async Task<PageResult<DailyHistoryGetResult>> Handle(DailyUserGetByDateCommand request, CancellationToken cancellationToken)
        {
            var dailyUsers = await _dailyUserService.GetDailyByDate(request.DateTime, request.SearchString, request.Page, request.PageSize);
            return dailyUsers;
        }

    }

}
