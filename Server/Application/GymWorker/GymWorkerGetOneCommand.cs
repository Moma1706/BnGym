using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymWorker;
using MediatR;

namespace Application.GymWorker
{
	public class GymWorkerGetOneCommand : IRequest<GymWorkerGetResult>
    {
        public Guid Id { get; set; }
    }

    public class GymWorkerGetOneCommandHandler : IRequestHandler<GymWorkerGetOneCommand, GymWorkerGetResult>
    {
        private readonly IGymWorkerService _gymWorkerService;

        public GymWorkerGetOneCommandHandler(IGymWorkerService gymWorkerService) => _gymWorkerService = gymWorkerService;

        public async Task<GymWorkerGetResult> Handle(GymWorkerGetOneCommand request, CancellationToken cancellationToken)
        {
            var gymWorkerResult = await _gymWorkerService.GetOne(request.Id);

            return gymWorkerResult;
        }

    }
}
