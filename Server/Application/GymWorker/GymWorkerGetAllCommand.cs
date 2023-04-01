using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymWorker;
using MediatR;

namespace Application.GymWorker
{
	public class GymWorkerGetAllCommand : IRequest<IList<GymWorkerGetResult>> {}

    public class GymWorkerGetAllCommandHandler : IRequestHandler<GymWorkerGetAllCommand, IList<GymWorkerGetResult>>
    {
        private readonly IGymWorkerService _gymWorkerService;

        public GymWorkerGetAllCommandHandler(IGymWorkerService gymWorkerService) => _gymWorkerService = gymWorkerService;

        public async Task<IList<GymWorkerGetResult>> Handle(GymWorkerGetAllCommand request, CancellationToken cancellationToken)
        {
            var gymWorkersResult = await _gymWorkerService.GetAll();
            return gymWorkersResult;
        }
    }
}
