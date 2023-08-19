using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Application.Common.Models.DailyUser;
using Application.Common.Models.GymUser;
using Application.DailyUser.Dtos;
using Application.Enums;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    internal class DailyUserService : IDailyUserService
    {
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;

        public DailyUserService(IConfiguration configuration, ApplicationDbContext dbContext, IDateTimeService dateTimeService, UserManager<User> userManager)
        {
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _dbContext = dbContext;
        }

        public async Task<DailyUserResult> Create(string firstName, string lastName, DateTime dateOfBirth)
        {
            var dailyUser = await _dbContext.DailyUser.FirstOrDefaultAsync((x) =>
                x.FirstName.ToLower() == firstName.ToLower() && x.LastName.ToLower() == lastName.ToLower() && x.DateOfBirth.Date == dateOfBirth.Date);

            if (dailyUser != null)
                return DailyUserResult.Failure(new Error { Code = ExceptionType.EntityAlreadyExists, Message = "Korisnik sa navedenim imenom, prezimenom i datumom rodjenja već postoji" });

            var newDailyUser = new DailyUser
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
                _dbContext.Add(newDailyUser);

                var dailyHistory = new DailyHistory
                {
                    DailyUserId = newDailyUser.Id,
                    CheckInDate = newDailyUser.LastCheckIn
                };
                _dbContext.Add(dailyHistory);

                _dbContext.SaveChanges();
                transaction.Commit();
                return DailyUserResult.Sucessfull();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                return DailyUserResult.Failure(new Error { Code = ExceptionType.UnableToCreate, Message = "Nije moguće sačuvati korisnika. " + exc.Message });
            }
        }

        public async Task<PageResult<DailyUserGetResult>> GetDailyUsers(string searchString, int page, int pageSize, SortOrder sortOrder)
        {
            page -= 1;
            if (page <= 0)
                page = 0;

            var query = _dbContext.DailyUserView.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString ?? ""));

            if (sortOrder == SortOrder.Ascending || sortOrder == SortOrder.Unspecified)
                query = query.OrderBy(x => x.FirstName);
            else
                query = query.OrderByDescending(x => x.FirstName);

            var list = query.ToList().Skip(page * pageSize).Take(pageSize).ToList();

            var currentMonth = DateTime.Now.Month.ToString();
            var lastMonth = DateTime.Now.AddMonths(-1).Month.ToString();

            var numberOfDayliArrivalsCurrentMonth = _dbContext.DailyHistoryView.Where(x => x.CheckInDate.Month.ToString() == currentMonth).Count();
            var numberOfDayliArrivalsLastMonth = _dbContext.DailyHistoryView.Where(x => x.CheckInDate.Month.ToString() == lastMonth).Count();

            return new PageResult<DailyUserGetResult>
            {
                Count = query.ToList().Count,
                PageIndex = page,
                PageSize = pageSize,
                ActiveCount = query.ToList().Count,
                NumberOfDayliArrivalsCurrentMonth = numberOfDayliArrivalsCurrentMonth,
                NumberOfDayliArrivalsLastMonth = numberOfDayliArrivalsLastMonth,
                Items = list.Select(x => new DailyUserGetResult()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    DateOfBirth = x.DateOfBirth,
                    LastCheckIn = x.LastCheckIn,
                    NumberOfArrivalsLastMonth = x.NumberOfArrivalsLastMonth,
                    NumberOfArrivalsCurrentMonth = x.numberOfArrivalsCurrentMonth
                }).ToList()
            };
        }

        public async Task<PageResult<DailyHistoryGetResult>> GetDailyByDate(DateTime date, string searchString, int page, int pageSize)
        {
            var dailyList = new List<DailyHistoryGetResult>();

            // prepare result
            var countDetails = _dbContext.DailyHistoryView.Count(x => x.CheckInDate.Date == date.Date);
            var result = new PageResult<DailyHistoryGetResult>
            {
                Count = countDetails,
                PageIndex = page,
                PageSize = pageSize,
                Items = dailyList,
                ActiveCount = 0
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

            var dailyUsers = await query.Where(x => x.CheckInDate.Date == date.Date).OrderBy(x => x.CheckInDate).ToListAsync();
            if (dailyUsers.Count == 0)
                return result;

            dailyList = dailyUsers.Select(x => new DailyHistoryGetResult()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                DateOfBirth = x.DateOfBirth,
                CheckInDate = x.CheckInDate
            }).ToList();

            result.Items = dailyList;
            return result;
        }

        public async Task<DailyUserResult> Update(Guid id, UpdateDailyUserDto data)
        {
            try
            {
                var dailyUser = await _dbContext.DailyUser.FirstOrDefaultAsync((x) => x.Id == id);
                if (dailyUser == null)
                    return DailyUserResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

                dailyUser.FirstName = data.FirstName ?? dailyUser.FirstName;
                dailyUser.LastName = data.LastName ?? dailyUser.LastName;
                if (data.DateOfBirth.Date != dailyUser.DateOfBirth.Date)
                {
                    var uniqueUser = _dbContext.DailyUser.Count((x) =>
                        x.FirstName.ToLower() == dailyUser.FirstName.ToLower() && x.LastName.ToLower() == dailyUser.LastName.ToLower() && x.DateOfBirth.Date == data.DateOfBirth.Date);
                    if (uniqueUser > 0)
                        return DailyUserResult.Failure(new Error { Code = ExceptionType.EntityAlreadyExists, Message = "Korisnik sa navedemin imenom, prezimenom i datumom rodjenja već postoji" });

                    dailyUser.DateOfBirth = data.DateOfBirth;
                }

                //update daily user
                _dbContext.Update(dailyUser);
                _dbContext.SaveChanges();

                return DailyUserResult.Sucessfull();
            }
            catch (Exception exc)
            {
                return DailyUserResult.Failure(new Error { Code = ExceptionType.UnableToUpdate, Message = "Nije moguće ažurirati korisnika. " + exc.Message });
            }
        }

        public async Task<DailyUserResult> AddArrival(Guid id)
        {
            var dailyUser = await _dbContext.DailyUser.FirstOrDefaultAsync((x) => x.Id == id);
            if (dailyUser == null)
                return DailyUserResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

            if (dailyUser.LastCheckIn.Date == _dateTimeService.Now.Date)
                return DailyUserResult.Failure(new Error { Code = ExceptionType.CanNotAccesTwice, Message = "Korisnik se ne može čekirati dva puta u toku dana" });

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                dailyUser.LastCheckIn = _dateTimeService.Now;
                _dbContext.Update(dailyUser);

                var dailyHistory = new DailyHistory
                {
                    DailyUserId = id,
                    CheckInDate = _dateTimeService.Now
                };
                _dbContext.Add(dailyHistory);
                _dbContext.SaveChanges();

                transaction.Commit();
                return DailyUserResult.Sucessfull();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                return DailyUserResult.Failure(new Error { Code = ExceptionType.UnableToCheckIn, Message = "Nije moguće evidentirati dolazak. " + exc.Message });
            }
        }

        public async Task<DailyUserGetResult> GetOne(Guid id)
        {
            var dailyUser = await _dbContext.DailyUserView.FirstOrDefaultAsync((x) => x.Id == id);
            if (dailyUser == null)
                return DailyUserGetResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

            return DailyUserGetResult.Sucessfull(
                dailyUser.Id,
                dailyUser.FirstName,
                dailyUser.LastName,
                dailyUser.DateOfBirth,
                dailyUser.LastCheckIn,
                dailyUser.numberOfArrivalsCurrentMonth,
                dailyUser.NumberOfArrivalsLastMonth
                );
        }
    }
}