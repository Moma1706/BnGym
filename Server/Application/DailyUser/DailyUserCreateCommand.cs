using System;
using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyUser;
using Application.Enums;
using MediatR;

namespace Application.DailyUser
{
    public record DailyUserCreateCommand : IRequest<DailyUserResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class DailyUserCreateCommandHandler : IRequestHandler<DailyUserCreateCommand, DailyUserResult>
    {
        private readonly IDailyUserService _dailyUserService;

        public DailyUserCreateCommandHandler(IDailyUserService dailyUserService) => _dailyUserService = dailyUserService;

        public async Task<DailyUserResult> Handle(DailyUserCreateCommand request, CancellationToken cancellationToken)
        {
            var dailyUserResult = await _dailyUserService.Create(request.FirstName, request.LastName, request.DateOfBirth);

            return dailyUserResult;
        }
    }
}

