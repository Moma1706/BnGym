using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Application.Common.Models.GymUser;
using Application.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    internal class CheckInService : ICheckInService
    {
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMaintenanceService _maintenanceService;

        public CheckInService(IConfiguration configuration, ApplicationDbContext dbContext, IDateTimeService dateTimeService, UserManager<User> userManager, IMaintenanceService maintenanceService)
        {
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _dbContext = dbContext;
            _maintenanceService = maintenanceService;
        }
        public async Task<CheckInResult> CheckIn(Guid gymUserId)
        {
            var gymUser = await _dbContext.GymUsers.FirstOrDefaultAsync(x => x.Id == gymUserId);
            if (gymUser == null)
                return CheckInResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Gym user with provided id does not exist" });

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == gymUser.UserId);
            if (user == null)
                return CheckInResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "User doesn't exist" });

            //var maintenanceResult = await _maintenanceService.CheckExpirationDate(user.Id);
            if (gymUser.IsFrozen)
                return CheckInResult.Failure(new Error { Code = ExceptionType.UserIsFrozen, Message = "Gym user has not a frozen membership" });

            if (gymUser.IsInActive)
                return CheckInResult.Failure(new Error { Code = ExceptionType.UserIsInActive, Message = "Gym user is inactive" });

            if (user.IsBlocked)
                return CheckInResult.Failure(new Error { Code = ExceptionType.UserIsBlocked, Message = "GymUser with provided id is blocked" });

            if (gymUser.LastCheckIn.Date == _dateTimeService.Now.Date)
                return CheckInResult.Failure(new Error { Code = ExceptionType.CanNotAccesTwice, Message = "GymUser with provided id can't access gym two times a day" });

            if (gymUser.ExpiresOn.Date < _dateTimeService.Now.Date)
                return CheckInResult.Failure(new Error { Code = ExceptionType.ExpiredMembership, Message = "GymUser with provided id has a membership that has expired" });

            var checkIn = new CheckInHistory { GymUserId = gymUserId, Id = Guid.NewGuid(), TimeStamp = _dateTimeService.Now };

            gymUser.LastCheckIn = checkIn.TimeStamp;

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                //update gymUser
                _dbContext.Update(gymUser);

                //save checkin
                _dbContext.Add(checkIn);
                _dbContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                return CheckInResult.Failure(new Error { Code = ExceptionType.UnableToCheckIn, Message = "Unable to add check in value" });
            }

            return CheckInResult.Sucessfull(checkIn.Id, checkIn.GymUserId, checkIn.TimeStamp);
        }

        public async Task<PageResult<CheckInGetResult>> GetCheckInsByDate(DateTime date, string searchString, int page, int pageSize)
        {
            var checkInList = new List<CheckInGetResult>();

            // prepare result
            var countDetails = _dbContext.CheckInHistoryView.Count(x => x.CheckInDate.Date == date.Date);
            var result = new PageResult<CheckInGetResult>
            {
                Count = countDetails,
                PageIndex = page,
                PageSize = pageSize,
                Items = checkInList
            };

            if (countDetails == 0)
                return result;

            page -= 1;
            if (page <= 0)
                page = 0;

            var query = _dbContext.CheckInHistoryView.Skip(page * pageSize).Take(pageSize);

            // applay searching string
            if (!String.IsNullOrEmpty(searchString))
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString));

            var checkIns = await query.Where(x => x.CheckInDate.Date == date.Date).OrderBy(x => x.CheckInDate).ToListAsync();
            if (checkIns.Count == 0)
                return result;

            checkInList = checkIns.Select(x => new CheckInGetResult()
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email == "email" ? "null" : x.Email ,
                CheckInDate = x.CheckInDate,
                GymUserId = x.GymUserId
            }).ToList();

            result.Items = checkInList;
            return result;
        }
    }
}
