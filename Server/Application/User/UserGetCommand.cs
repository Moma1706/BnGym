using System;
using Application.Common.Interfaces;
using Application.Common.Models.User;
using MediatR;

namespace Application.User
{
    public class UserGetCommand : IRequest<UserGetResult>
    {
        public int Id { get; set; }
    }

    public class UserGetCommandHandler : IRequestHandler<UserGetCommand, UserGetResult>
    {
        private readonly IGymWorkerService _gymWorkerService;

        public UserGetCommandHandler(IGymWorkerService gymWorkerService) => _gymWorkerService = gymWorkerService;

        public async Task<UserGetResult> Handle(UserGetCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _gymWorkerService.GetUser(request.Id);

            return userResult;
        }

    }
}
