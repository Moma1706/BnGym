using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymWorker;
using MediatR;

namespace Application.GymWorker
{
	public class GymWorkerDeleteCommand : IRequest<GymWorkerResult>
    {
        public Guid Id { get; set; }
    }

    public class GymWorkerDeleteCommandHandler : IRequestHandler<GymWorkerDeleteCommand, GymWorkerResult>
    {
        private readonly IGymWorkerService _gymWorkerService;

        public GymWorkerDeleteCommandHandler(IGymWorkerService gymWorkerService) => _gymWorkerService = gymWorkerService;

        public async Task<GymWorkerResult> Handle(GymWorkerDeleteCommand request, CancellationToken cancellationToken)
        {
            var gymWorkerResult = await _gymWorkerService.Delete(request.Id);

            return gymWorkerResult;
        }
    }
}
