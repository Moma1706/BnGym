using Application.Common.Interfaces;
using Application.Common.Models.DailyTraining;
using Application.Enums;
using MediatR;

namespace Application.DailyTraining
{
    public record DailyTrainingGetOneCommand : IRequest<DailyTrainingGetResult>
    {
        public Guid Id { get; set; }
    }

    public class DailyTrainingGetOneCommandHandler : IRequestHandler<DailyTrainingGetOneCommand, DailyTrainingGetResult>
    {
        private readonly IDailyTrainingService _dailyTrainingService;

        public DailyTrainingGetOneCommandHandler(IDailyTrainingService dailyTrainingService) => _dailyTrainingService = dailyTrainingService;

        public async Task<DailyTrainingGetResult> Handle(DailyTrainingGetOneCommand request, CancellationToken cancellationToken)
        {
            var result = await _dailyTrainingService.GetOne(request.Id);

            return result;
        }

    }
}
