using Application.Common.Interfaces;
using Application.Common.Models.DailyTraining;
using Application.Enums;
using MediatR;

namespace Application.DailyTraining
{
    public record DailyTrainingAddArrivalCommand : IRequest<DailyTrainingResult>
    {
        public Guid Id { get; set; }
    }

    public class GymUserGetOneCommandHandler : IRequestHandler<DailyTrainingAddArrivalCommand, DailyTrainingResult>
    {
        private readonly IDailyTrainingService _dailyTrainingService;

        public GymUserGetOneCommandHandler(IDailyTrainingService dailyTrainingService) => _dailyTrainingService = dailyTrainingService;

        public async Task<DailyTrainingResult> Handle(DailyTrainingAddArrivalCommand request, CancellationToken cancellationToken)
        {
            var result = await _dailyTrainingService.AddArrival(request.Id);

            return result;
        }

    }
}
