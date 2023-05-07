using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyUser;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;
using Microsoft.Data.SqlClient;

namespace Application.DailyUser
{
    public record DailyUserGetAllCommand : IRequest<PageResult<DailyUserGetResult>>
    {
        public string SearchString { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

    }

    public class DailyUserGetAllCommandHandler : IRequestHandler<DailyUserGetAllCommand, PageResult<DailyUserGetResult>>
    {
        private readonly IDailyUserService _dailyUserService;

        public DailyUserGetAllCommandHandler(IDailyUserService dailyUserService) => _dailyUserService = dailyUserService;

        public async Task<PageResult<DailyUserGetResult>> Handle(DailyUserGetAllCommand request, CancellationToken cancellationToken)
        {
            var result = await _dailyUserService.GetDailyUsers(request.SearchString, request.Page, request.PageSize, request.SortOrder);
            return result;
        }

    }
}
