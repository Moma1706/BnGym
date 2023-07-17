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
        private readonly IEmailService _emailService;

        public UserGetCommandHandler(IGymWorkerService gymWorkerService, IEmailService emailService)
        {
            _gymWorkerService = gymWorkerService;
            _emailService = emailService;
        }

        public async Task<UserGetResult> Handle(UserGetCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _gymWorkerService.GetUser(request.Id);
            await _emailService.SendEmailSubscriptionexpires("", "");

            return userResult;
        }
    }
}