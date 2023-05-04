using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Application.Common.Models.GymUser;
using MediatR;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CheckIn
{
    
    public record CheckInGetByDateCommand : IRequest<PageResult<CheckInGetResult>> {
        public DateTime DateTime { get; set; }

        public string SearchString { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;
    }
        
    public class GymUserGetAllCommandHandler : IRequestHandler<CheckInGetByDateCommand, PageResult<CheckInGetResult>>
    {
        private readonly ICheckInService _checkInService;

        public GymUserGetAllCommandHandler(ICheckInService checkInService) => _checkInService = checkInService;

        public async Task<PageResult<CheckInGetResult>> Handle(CheckInGetByDateCommand request, CancellationToken cancellationToken)
        {
            var checkIns = await _checkInService.GetCheckInsByDate(request.DateTime, request.SearchString, request.Page, request.PageSize, request.SortOrder);
            return checkIns;
        }

    }
    
}
