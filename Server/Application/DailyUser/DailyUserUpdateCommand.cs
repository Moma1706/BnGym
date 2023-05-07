using System;
using Application.Common.Interfaces;
using Application.Common.Models.DailyUser;
using Application.DailyUser.Dtos;
using Application.Enums;
using MediatR;

namespace Application.DailyUser
{
    public record DailyUserUpdateCommand : IRequest<DailyUserResult>
    {
        public Guid Id { get; set; }
        public UpdateDailyUserDto Data { get; set; }
    }

    public class DailyUserUpdateCommandHandler : IRequestHandler<DailyUserUpdateCommand, DailyUserResult>
    {
        private readonly IDailyUserService _dailyUserService;

        public DailyUserUpdateCommandHandler(IDailyUserService dailyUserService) => _dailyUserService = dailyUserService;

        public async Task<DailyUserResult> Handle(DailyUserUpdateCommand request, CancellationToken cancellationToken)
        {
            var dailyUserResult = await _dailyUserService.Update(request.Id, request.Data);

            return dailyUserResult;
        }
    }
}

