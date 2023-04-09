using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyTraining;
using Application.DailyTraining.Dtos;
using Infrastructure.Data;
using MediatR;
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
    internal class DailyTrainingService : IDailyTrainingService
    {
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;

        public DailyTrainingService(IConfiguration configuration, ApplicationDbContext dbContext, IDateTimeService dateTimeService, UserManager<User> userManager)
        {
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _dbContext = dbContext;
        }

        public async Task<DailyTrainingResult> Create(string firstName, string lastName, DateTime dateOfBirth)
        {
            try
            {
                var dailyUser = await _dbContext.DailyTraining.FirstOrDefaultAsync((x) =>
                    x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower() && x.DateOfBirth.Date == dateOfBirth.Date);

                if (dailyUser != null) // dodaj novi
                    return DailyTrainingResult.Failure("User already exists");

                var dailyTraining = new DailyTraining
                {
                    Id = Guid.NewGuid(),
                    CheckInDate = _dateTimeService.Now,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    NumberOfArrivals = 1
                };

                _dbContext.Add(dailyTraining);
                _dbContext.SaveChanges();
                return DailyTrainingResult.Sucessfull();
            }
            catch (Exception)
            {
                return DailyTrainingResult.Failure("Unable to add daily training");
            }
        }

        public async Task<PageResult<DailyUsersGetResult>> GetDailyUsers(string searchString, int page, int pageSize)
        {
            var dailyList = new List<DailyUsersGetResult>();

            // prepare result
            var countDetails = _dbContext.DailyTraining.Count();
            var result = new PageResult<DailyUsersGetResult>
            {
                Count = countDetails,
                PageIndex = page,
                PageSize = pageSize,
                Items = dailyList
            };

            if (countDetails == 0)
                return result;

            if (page - 1 <= 0)
                page = 0;

            var query = _dbContext.DailyTraining.Skip(page * pageSize).Take(pageSize);

            // applay searching string
            if (!String.IsNullOrEmpty(searchString))
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString));

            var dailyTrainings = await query.OrderBy(x => x.CheckInDate).ToListAsync();
            if (dailyTrainings.Count == 0)
                return result;

            dailyList = dailyTrainings.Select(x => new DailyUsersGetResult()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth
            }).ToList();

            result.Items = dailyList;
            return result;

        }

        public async Task<PageResult<DailyTrainingGetResult>> GetDailyByDate(DateTime date, string searchString, int page, int pageSize)
        {

            var dailyList = new List<DailyTrainingGetResult>();

            // prepare result
            var countDetails = _dbContext.DailyTraining.Count();
            var result = new PageResult<DailyTrainingGetResult>
            {
                Count = countDetails,
                PageIndex = page,
                PageSize = pageSize,
                Items = dailyList
            };

            if (countDetails == 0)
                return result;

            if (page - 1 <= 0)
                page = 0;

            var query = _dbContext.DailyTraining.Skip(page * pageSize).Take(pageSize);

            // applay searching string
            if (!String.IsNullOrEmpty(searchString))
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString));

            var dailyTrainings = await query.Where(x => x.CheckInDate.Date == date.Date).OrderBy(x => x.CheckInDate).ToListAsync();
            if (dailyTrainings.Count == 0)
                return result;

            dailyList = dailyTrainings.Select(x => new DailyTrainingGetResult()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                CheckInDate = x.CheckInDate,
                DateOfBirth = x.DateOfBirth,
                NumberOfArrivals = x.NumberOfArrivals
            }).ToList();

            result.Items = dailyList;
            return result;

        }

        public async Task<DailyTrainingResult> Update(Guid id, UpdateDailyTrainingDto data)
        {
            try
            {
                var dailyUser = await _dbContext.DailyTraining.FirstOrDefaultAsync((x) => x.Id == id);
                if (dailyUser == null)
                    return DailyTrainingResult.Failure("Daily user does not exist");

                // TODO: Provjeriti ??
                dailyUser.FirstName = data.FirstName ?? dailyUser.FirstName;
                dailyUser.LastName = data.LastName ?? dailyUser.LastName;
                if (data.DateOfBirth is string)
                {
                    try
                    {
                        var date = Convert.ToDateTime(data.DateOfBirth);
                        dailyUser.DateOfBirth = date;
                    }
                    catch (Exception)
                    {
                        DailyTrainingResult.Failure("Unable to convert date string to DateTime");
                    }
                }

                //update daily user
                _dbContext.Update(dailyUser);
                _dbContext.SaveChanges();

                return DailyTrainingResult.Sucessfull();

            }
            catch (Exception)
            {
                return DailyTrainingResult.Failure("Unable to update daily training");
            }
        }

        public async Task<DailyTrainingResult> AddArrival(Guid id)
        {
            var dailyUser = await _dbContext.DailyTraining.FirstOrDefaultAsync((x) => x.Id == id);
            if (dailyUser == null)
                return DailyTrainingResult.Failure("Daily user does not exist");

            if (dailyUser.CheckInDate.Date == _dateTimeService.Now.Date)
                return DailyTrainingResult.Failure("Daily user can't access gym two times a day");

            if (dailyUser.CheckInDate.Month == _dateTimeService.Now.Date.Month &&
                dailyUser.CheckInDate.Year == _dateTimeService.Now.Date.Year)
                dailyUser.NumberOfArrivals++;
            else
                dailyUser.NumberOfArrivals = 1;

            dailyUser.CheckInDate = _dateTimeService.Now;

            //update daily user
            _dbContext.Update(dailyUser);
            _dbContext.SaveChanges();
            return DailyTrainingResult.Sucessfull();
        }

        public Task<DailyTrainingGetResult> GetOne(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
