using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Application.Common.Models.GymUser;
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
                return CheckInResult.Failure("GymUser with provided id doesn't exist");

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == gymUser.UserId);
            if (user == null)
                return CheckInResult.Failure("User doesn't exist");

            if (gymUser.IsFrozen)
                return CheckInResult.Failure("GymUser with provided id is frozen");

            if (gymUser.IsInActive)
                return CheckInResult.Failure("GymUser with provided id is inactive");

            if (user.IsBlocked)
                return CheckInResult.Failure("GymUser with provided id is blocked");

            if (gymUser.LastCheckIn.Date == _dateTimeService.Now.Date)
                return CheckInResult.Failure("GymUser with provided id can't access gym two times a day");

            if (gymUser.ExpiresOn.Date < _dateTimeService.Now.Date)
                return CheckInResult.Failure("GymUser with provided id has a membership that has expired");

            var checkIn = new CheckInHistory { GymUserId = gymUserId, Id = Guid.NewGuid(), TimeStamp = _dateTimeService.Now };

            gymUser.NumberOfArrivals++;
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
                return CheckInResult.Failure("Unable to add check in value");
            }

            return CheckInResult.Sucessfull(checkIn.Id, checkIn.GymUserId, checkIn.TimeStamp);
        }

        public async Task<PageResult<CheckInGetResult>> GetCheckInsByDate(DateTime date, string searchString, int page, int pageSize)
        {
            var checkInList = new List<CheckInGetResult>();

            // prepare result
            var countDetails = _dbContext.CheckInHistoryView.Count();
            var result = new PageResult<CheckInGetResult>
            {
                Count = countDetails,
                PageIndex = page,
                PageSize = pageSize,
                Items = checkInList
            };

            if (countDetails == 0)
                return result;

            if (page - 1 <= 0)
                page = 0;

            var query = _dbContext.CheckInHistoryView.Skip(page * pageSize).Take(pageSize);

            // applay searching string
            if (!String.IsNullOrEmpty(searchString))
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString));

            var checkIns = await query.Where(x => x.TimeStamp.Date == date.Date).OrderBy(x => x.LastCheckIn).ToListAsync();
            if (checkIns.Count == 0)
                return result;

            checkInList = checkIns.Select(x => new CheckInGetResult()
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                LastCheckIn = x.LastCheckIn,
                TimeStamp = x.TimeStamp,
                NumberOfArrivals = x.NumberOfArrivals
            }).ToList();

            result.Items = checkInList;
            return result;
        }
    }
}
