using Application.Common.Interfaces;
using Application.Common.Models.CheckIn;
using Application.Common.Models.GymUser;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CheckIn
{
    
    public record CheckInGetByDateCommand : IRequest<IList<CheckInGetResult>> {
        public DateTime DateTime { get; set; }
    }
        
    public class GymUserGetAllCommandHandler : IRequestHandler<CheckInGetByDateCommand, IList<CheckInGetResult>>
    {
        private readonly ICheckInService _checkInService;

        public GymUserGetAllCommandHandler(ICheckInService checkInService) => _checkInService = checkInService;

        public async Task<IList<CheckInGetResult>> Handle(CheckInGetByDateCommand request, CancellationToken cancellationToken)
        {
            var checkIns = await _checkInService.GetCheckInsByDate(request.DateTime);
            return checkIns;
        }

    }
    
}
