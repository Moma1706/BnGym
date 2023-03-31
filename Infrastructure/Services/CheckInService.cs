﻿using Application.Common.Interfaces;
using Application.Common.Models.Auth;
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

        public CheckInService(IConfiguration configuration, ApplicationDbContext dbContext, IDateTimeService dateTimeService, UserManager<User> userManager)
        {
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _dbContext = dbContext;
        }
        public async Task<CheckInResult> CheckIn(Guid gymUserId)
        {
            var gymUser = await _dbContext.GymUsers.FirstOrDefaultAsync(x => x.Id == gymUserId);

            if (gymUser == null)
                return CheckInResult.Failure("GymUser with provided id doesn't exist");

            if (gymUser.IsFrozen)
                return CheckInResult.Failure("GymUser with provided id is frozen");

            if (gymUser.IsInActive)
                return CheckInResult.Failure("GymUser with provided id is inactive");

            if(gymUser.LastCheckIn == DateTime.UtcNow)
                return CheckInResult.Failure("GymUser with provided id can't access gym two times a day");

            if (gymUser.ExpiresOn < DateTime.UtcNow)
                return CheckInResult.Failure("GymUser with provided id has a membership that has expired");

            var checkIn = new CheckInHistory { GymUserId = gymUserId, Id = Guid.NewGuid(), TimeStamp = _dateTimeService.Now };

            gymUser.NumberOfArrivals++;
            gymUser.LastCheckIn = checkIn.TimeStamp;

            using var transaction = _dbContext.Database.BeginTransaction();

            try
            {
                //update gymuser
                _dbContext.Update(gymUser);

                //save checkin
                _dbContext.Add(checkIn);
                _dbContext.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                return CheckInResult.Failure("Unable to add chek in");
            }

            return CheckInResult.Sucessfull(checkIn.Id, checkIn.GymUserId,checkIn.TimeStamp);
        }

        public async Task<IList<CheckInGetResult>> GetCheckInsByDate(DateTime date) //uraditi paginaciju sortiranje i filtriranje
        {
            var checkInsReturn = new List<CheckInGetResult>();

            var checkIns = await _dbContext.CheckIns.Where(x => x.TimeStamp.Date == date.Date).ToListAsync();
            if (checkIns.Count == 0)
                return checkInsReturn;

            var gymUserIds = checkIns.Select(x => x.GymUserId);
            var gymUsers = _dbContext.GymUsers.Where(u => gymUserIds.Contains(u.Id)).ToList();

            var userIds = gymUsers.Select(x => x.UserId);
            var users = _dbContext.Users.Where(u => userIds.Contains(u.Id)).ToList();

            foreach (var checkin in checkIns)
            {
                var checkinGet = checkIns.Where(x => x.Id == checkin.Id).FirstOrDefault(); 
                if (checkinGet != null)
                {
                    var gymUser = gymUsers.Where(x => x.Id == checkinGet.GymUserId).FirstOrDefault();
                    var user = users.Where(x => x.Id == gymUser.UserId).FirstOrDefault();
                    var Ci = new CheckInGetResult()
                    {
                        Id = checkinGet.Id,
                        GymUserId = checkinGet.GymUserId,
                        TimeStamp = checkinGet.TimeStamp,
                        FirstName = user.FirstName,
                        LastName = user.LastName
                    };
                    checkInsReturn.Add(Ci);
                }
            }

            return checkInsReturn;
        }
    }
}
