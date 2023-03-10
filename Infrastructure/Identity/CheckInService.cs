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
        public async Task<CheckInResult> CheckIn(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return CheckInResult.Failure($"User with Id = {userId}  doesn't exist");

            var checkIn = new CheckInHistory { UserId = userId, Id = Guid.NewGuid(), TimeStamp = _dateTimeService.Now };

            _dbContext.Add(checkIn);
            _dbContext.SaveChanges();
                
            return CheckInResult.Sucessfull(checkIn.Id, checkIn.UserId,checkIn.TimeStamp);
        }
    }
}
