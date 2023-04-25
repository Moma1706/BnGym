using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Application.Common.Models.DailyTraining;
using Application.Common.Models.GymUser;
using Application.DailyTraining.Dtos;
using Application.Enums;
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
            var dailyUser = await _dbContext.DailyTraining.FirstOrDefaultAsync((x) =>
                x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower() && x.DateOfBirth.Date == dateOfBirth.Date);

            if (dailyUser != null)
                return DailyTrainingResult.Failure(new Error { Code = ExceptionType.EntityAlreadyExists, Message = "User already exists" });

            var dailyTraining = new DailyTraining
            {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                LastCheckIn = _dateTimeService.Now
            };

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Add(dailyTraining);

                var dailyHistory = new DailyHistory
                {
                    DailyUserId = dailyTraining.Id,
                    CheckInDate = dailyTraining.LastCheckIn
                };
                _dbContext.Add(dailyHistory);

                _dbContext.SaveChanges();
                transaction.Commit();
                return DailyTrainingResult.Sucessfull();
            }
            catch (Exception)
            {
                transaction.Rollback();
                return DailyTrainingResult.Failure(new Error { Code = ExceptionType.UnableToCreate, Message = "Unable to add daily training" });
            }
        }

        public async Task<PageResult<DailyTrainingGetResult>> GetDailyUsers(string searchString, int page, int pageSize)
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

            page -= 1;
            if (page <= 0)
                page = 0;

            var query = _dbContext.DailyTraining.Skip(page * pageSize).Take(pageSize);

            // applay searching string
            if (!String.IsNullOrEmpty(searchString))
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString));

            var dailyTrainings = await query.OrderBy(x => x.FirstName).ToListAsync();
            if (dailyTrainings.Count == 0)
                return result;

            dailyList = dailyTrainings.Select(x => new DailyTrainingGetResult()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                LastCheckIn = x.LastCheckIn
            }).ToList();

            result.Items = dailyList;
            return result;

        }

        public async Task<PageResult<DailyHistoryGetResult>> GetDailyByDate(DateTime date, string searchString, int page, int pageSize)
        {
            var dailyList = new List<DailyHistoryGetResult>();

            // prepare result
            var countDetails = _dbContext.DailyHistoryView.Count();
            var result = new PageResult<DailyHistoryGetResult>
            {
                Count = countDetails,
                PageIndex = page,
                PageSize = pageSize,
                Items = dailyList
            };

            if (countDetails == 0)
                return result;

            page -= 1;
            if (page <= 0)
                page = 0;

            var query = _dbContext.DailyHistoryView.Skip(page * pageSize).Take(pageSize);

            // applay searching string
            if (!String.IsNullOrEmpty(searchString))
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString));

            var dailyTrainings = await query.Where(x => x.CheckInDate.Date == date.Date).OrderBy(x => x.CheckInDate).ToListAsync();
            if (dailyTrainings.Count == 0)
                return result;

            dailyList = dailyTrainings.Select(x => new DailyHistoryGetResult()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                NumberOfArrivalsCurrentMonth = x.NumberOfArrivalsCurrentMonth,
                NumberOfArrivalsLastMonth = x.NumberOfArrivalsLastMonth,
                CheckInDate = x.CheckInDate
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
                    return DailyTrainingResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Daily user does not exist" });

                dailyUser.FirstName = data.FirstName ?? dailyUser.FirstName;
                dailyUser.LastName = data.LastName ?? dailyUser.LastName;
                if (data.DateOfBirth.Date != dailyUser.DateOfBirth.Date)
                    dailyUser.DateOfBirth = data.DateOfBirth;

                //update daily user
                _dbContext.Update(dailyUser);
                _dbContext.SaveChanges();

                return DailyTrainingResult.Sucessfull();
            }
            catch (Exception)
            {
                return DailyTrainingResult.Failure(new Error { Code = ExceptionType.UnableToUpdate, Message = "Unable to update daily user" });
            }
        }

        public async Task<DailyTrainingResult> AddArrival(Guid id)
        {
            var dailyUser = await _dbContext.DailyTraining.FirstOrDefaultAsync((x) => x.Id == id);
            if (dailyUser == null)
                return DailyTrainingResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Daily user does not exist" });

            var dailyHistory = await _dbContext.DailyHistory.FirstOrDefaultAsync((x) => x.Id == id);

            if (dailyHistory.CheckInDate.Date == _dateTimeService.Now.Date)
                return DailyTrainingResult.Failure(new Error { Code = ExceptionType.CanNotAccesTwice, Message = "Daily user can't access gym two times a day" });

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                dailyUser.LastCheckIn = DateTime.Now;
                _dbContext.Update(dailyUser);

                dailyHistory = new DailyHistory
                {
                    DailyUserId = id,
                    CheckInDate = DateTime.Now
                };
                _dbContext.Add(dailyHistory);
                _dbContext.SaveChanges();
                return DailyTrainingResult.Sucessfull();

            } catch (Exception)
            {
                transaction.Rollback();
                return DailyTrainingResult.Failure(new Error { Code = ExceptionType.UnableToCheckIn, Message = "Unable to add check in value" });
            }
        }

        public async Task<DailyTrainingGetResult> GetOne(Guid id)
        {
            var dailyUser = await _dbContext.DailyTraining.FirstOrDefaultAsync((x) => x.Id == id);
            if (dailyUser == null)
                return DailyTrainingGetResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Daily user does not exist" });

            return DailyTrainingGetResult.Sucessfull(
                dailyUser.Id,
                dailyUser.FirstName,
                dailyUser.LastName,
                dailyUser.DateOfBirth,
                dailyUser.LastCheckIn
                );
        }
    }
}
