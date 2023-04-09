using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymWorker;
using Application.GymWorker.Dtos;
using MediatR;

namespace Application.GymWorker
{
	public class GymWorkerUpdateCommand : IRequest<GymWorkerResult>
    {
        public Guid Id { get; set; }
        public UpdateGymWorkerDto Data { get; set; }

        public class GymWorkerUpdateCommandHandler : IRequestHandler<GymWorkerUpdateCommand, GymWorkerResult>
        {
            private readonly IGymWorkerService _gymWorkerService;

            public GymWorkerUpdateCommandHandler(IGymWorkerService gymWorkerService) => _gymWorkerService = gymWorkerService;

            public async Task<GymWorkerResult> Handle(GymWorkerUpdateCommand data, CancellationToken cancellationToken)
            {
                var gymWorkerResult = await _gymWorkerService.Update(data.Id, data.Data);

                return gymWorkerResult;
            }
        }
    }
}

