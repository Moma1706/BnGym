using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymWorker;
using MediatR;

namespace Application.GymWorker
{
    public class GymWorkerActivateCommand : IRequest<GymWorkerResult>
    {
        public Guid Id { get; set; }
    }

    public class GymWorkerActivateCommandHandler : IRequestHandler<GymWorkerActivateCommand, GymWorkerResult>
    {
        private readonly IGymWorkerService _gymWorkerService;

        public GymWorkerActivateCommandHandler(IGymWorkerService gymWorkerService) => _gymWorkerService = gymWorkerService;

        public async Task<GymWorkerResult> Handle(GymWorkerActivateCommand request, CancellationToken cancellationToken)
        {
            var gymWorkerResult = await _gymWorkerService.Activate(request.Id);

            return gymWorkerResult;
        }
    }
}

