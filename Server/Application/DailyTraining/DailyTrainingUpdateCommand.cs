using System;
using Application.Common.Interfaces;
using Application.Common.Models.DailyTraining;
using Application.DailyTraining.Dtos;
using Application.Enums;
using MediatR;

namespace Application.DailyTraining
{
    public record DailyTrainingUpdateCommand : IRequest<DailyTrainingResult>
    {
        public Guid Id { get; set; }
        public UpdateDailyTrainingDto Data { get; set; }
    }

    public class DailyTrainingUpdateCommandHandler : IRequestHandler<DailyTrainingUpdateCommand, DailyTrainingResult>
    {
        private readonly IDailyTrainingService _dailyTrainingService;

        public DailyTrainingUpdateCommandHandler(IDailyTrainingService dailyTrainingService) => _dailyTrainingService = dailyTrainingService;

        public async Task<DailyTrainingResult> Handle(DailyTrainingUpdateCommand request, CancellationToken cancellationToken)
        {
            var dailyTrainingResult = await _dailyTrainingService.Update(request.Id, request.Data);

            return dailyTrainingResult;
        }
    }
}

