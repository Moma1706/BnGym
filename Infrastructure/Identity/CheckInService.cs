using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.CheckIn;
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
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _dbContext;

        public CheckInService(IConfiguration configuration, ApplicationDbContext dbContext, IDateTimeService dateTimeService, UserManager<User> userManager)
        {
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _userManager = userManager;
            _dbContext = dbContext;
        }
        public async Task<CheckInResult> CheckIn(Guid gymUserId)
        {
            var gymUser = await _dbContext.GymUsers.FirstOrDefaultAsync(x => x.Id == gymUserId);

            if (gymUser == null)
                return CheckInResult.Failure($" GymUser with UserId = {gymUserId}  doesn't exist");

            if (gymUser.IsFrozen)
                return CheckInResult.Failure($" GymUser with UserId = {gymUserId}  is frozen");

            if (gymUser.IsInActive)
                return CheckInResult.Failure($" GymUser with UserId = {gymUserId}  is inactive");

            if(gymUser.LastCheckIn == DateTime.UtcNow)
                return CheckInResult.Failure($" GymUser with UserId = {gymUserId}  can't access gym two times a day");

            if (gymUser.ExpiresOn < DateTime.UtcNow)
                return CheckInResult.Failure($" GymUser with UserId = {gymUserId}  has a membership that has expired");

            var checkIn = new CheckInHistory { GymUserId = gymUserId, Id = Guid.NewGuid(), TimeStamp = _dateTimeService.Now };

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                gymUser.NumberOfTrainingsLeft--;
                gymUser.NumberOfArrivals++;
                gymUser.LastCheckIn = checkIn.TimeStamp;
                //update gymuser

                _dbContext.Update(gymUser);
                _dbContext.Add(checkIn);
                _dbContext.SaveChanges();
                //save checkin

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }

            return CheckInResult.Sucessfull(checkIn.Id, checkIn.GymUserId,checkIn.TimeStamp);
        }
    }
}
