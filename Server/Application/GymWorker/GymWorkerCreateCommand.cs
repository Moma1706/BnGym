using System;
using Application.Common.Interfaces;
using Application.Common.Models.GymWorker;
using MediatR;

namespace Application.GymWorker
{
    public class GymWorkerCreateCommand : IRequest<GymWorkerResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }

    public class GymWorkerCreateCommandHandler : IRequestHandler<GymWorkerCreateCommand, GymWorkerResult>
    {
        private readonly IGymWorkerService _gymWorkerService;

        public GymWorkerCreateCommandHandler(IGymWorkerService gymWorkerService) => _gymWorkerService = gymWorkerService;

        public async Task<GymWorkerResult> Handle(GymWorkerCreateCommand request, CancellationToken cancellationToken)
        {
            var gymWorkerResult = await _gymWorkerService.Create(request.FirstName, request.LastName, request.Email);

            return gymWorkerResult;
        }

    }
}
