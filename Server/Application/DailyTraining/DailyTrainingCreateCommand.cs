using System;
using Application.Common.Interfaces;
using Application.Common.Models.DailyTraining;
using Application.Enums;
using MediatR;

namespace Application.DailyTraining
{
    public record DailyTrainingCreateCommand : IRequest<DailyTrainingResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
    }

    public class DailyTrainingCreateCommandHandler : IRequestHandler<DailyTrainingCreateCommand, DailyTrainingResult>
    {
        private readonly IDailyTrainingService _dailyTrainingService;

        public DailyTrainingCreateCommandHandler(IDailyTrainingService dailyTrainingService) => _dailyTrainingService = dailyTrainingService;

        public async Task<DailyTrainingResult> Handle(DailyTrainingCreateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var date = Convert.ToDateTime(request.DateOfBirth);
                var dailyTrainingResult = await _dailyTrainingService.Create(request.FirstName, request.LastName, date);

                return dailyTrainingResult;
            }
            catch (Exception)
            {
                return DailyTrainingResult.Failure("Unable to convert date string to DateTime");
            }
        }
    }
}

