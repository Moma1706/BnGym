using System;
using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyTraining;
using Application.Enums;
using MediatR;

namespace Application.DailyTraining
{
    public record DailyTrainingCreateCommand : IRequest<DailyTrainingResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class DailyTrainingCreateCommandHandler : IRequestHandler<DailyTrainingCreateCommand, DailyTrainingResult>
    {
        private readonly IDailyTrainingService _dailyTrainingService;

        public DailyTrainingCreateCommandHandler(IDailyTrainingService dailyTrainingService) => _dailyTrainingService = dailyTrainingService;

        public async Task<DailyTrainingResult> Handle(DailyTrainingCreateCommand request, CancellationToken cancellationToken)
        {
            var dailyTrainingResult = await _dailyTrainingService.Create(request.FirstName, request.LastName, request.DateOfBirth);

            return dailyTrainingResult;
        }
    }
}

