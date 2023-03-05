using Application.Common.Interfaces;
using Application.Common.Models.CheckIn;
using MediatR;

namespace Application.CheckIn.CheckIn
{
    public record CheckInCommand : IRequest<CheckInResult>
    {
        public int UsertId { get; set; }
    }

    public class CheckInCommandHandler : IRequestHandler<CheckInCommand, CheckInResult>
    {
        private readonly ICheckInService _checkInService;

        public CheckInCommandHandler(ICheckInService checkInService) => _checkInService = checkInService;

        public async Task<CheckInResult> Handle(CheckInCommand request, CancellationToken cancellationToken)
        {
            var checkinResult = await _checkInService.CheckIn(request.UsertId);

            return checkinResult;
        }

    }
}
