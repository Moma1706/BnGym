using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Application.Enums;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Identity
{
    internal class CheckInService : ICheckInService
    {
        private readonly IConfiguration _configuration;
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMaintenanceService _maintenanceService;
        private readonly INotificationService _notificationService;

        public CheckInService(IConfiguration configuration, ApplicationDbContext dbContext, IDateTimeService dateTimeService,
            UserManager<User> userManager, IMaintenanceService maintenanceService, INotificationService notificationService)
        {
            _configuration = configuration;
            _dateTimeService = dateTimeService;
            _dbContext = dbContext;
            _maintenanceService = maintenanceService;
            _notificationService = notificationService;
        }

        public async Task<CheckInResult> CheckIn(Guid gymUserId)
        {
            string message = "";
            var gymUser = await _dbContext.GymUsers.FirstOrDefaultAsync(x => x.Id == gymUserId);
            if (gymUser == null)
                return CheckInResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Gym korisnik sa proslijedjenim id ne postoji" });

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == gymUser.UserId);
            if (user == null)
                return CheckInResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik ne postoji" });

            // Check expiration date
            var maintenanceResult = await _maintenanceService.CheckExpirationDate(user.Id);

            if (gymUser.IsFrozen)
            {
                message = user.FirstName + " " + user.LastName + " se čekiro/la. Status: Zaleđen. Vrijeme: " + DateTime.Now.ToString();
                await _notificationService.Add(message);

                return CheckInResult.Failure(new Error { Code = ExceptionType.UserIsFrozen, Message = "Korisnik je zaledjen" });
            }

            if (gymUser.IsInActive)
            {
                message = user.FirstName + " " + user.LastName + " se čekiro/la. Status: Neaktivan. Vrijeme: " + DateTime.Now.ToString();
                await _notificationService.Add(message);

                return CheckInResult.Failure(new Error { Code = ExceptionType.UserIsInActive, Message = "Korisnik je neaktivan" });
            }

            if (user.IsBlocked)
            {
                message = user.FirstName + " " + user.LastName + " se čekiro/la. Status: Blokiran. Vrijeme: " + DateTime.Now.ToString();
                await _notificationService.Add(message);

                return CheckInResult.Failure(new Error { Code = ExceptionType.UserIsBlocked, Message = "Korisnik je blokiran" });
            }

            if (gymUser.LastCheckIn.Date == _dateTimeService.Now.Date)
            {
                message = user.FirstName + " " + user.LastName + " se čekiro/la. Status: Korisnik je već jednom čekiran u toku današenjeg dana. Vrijeme: " + DateTime.Now.ToString();
                await _notificationService.Add(message);

                return CheckInResult.Failure(new Error { Code = ExceptionType.CanNotAccesTwice, Message = "Korisnik se ne može čekirati dva puta u toku dana" });
            }

            if (gymUser.ExpiresOn.Date < _dateTimeService.Now.Date)
            {
                message = user.FirstName + " " + user.LastName + " se čekirao/la. Status: Neaktivan. Vrijeme: " + DateTime.Now.ToString();
                await _notificationService.Add(message);

                return CheckInResult.Failure(new Error { Code = ExceptionType.ExpiredMembership, Message = "Korisniku je istekla članarina" });
            }

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

                message = user.FirstName + " " + user.LastName + " se čekiro/la. Status: Aktivan. Vrijeme: " + DateTime.Now.ToString();
                await _notificationService.Add(message);
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                return CheckInResult.Failure(new Error { Code = ExceptionType.UnableToCheckIn, Message = "Nije moguće evidentirati dolazak. " + exc.Message });
            }

            return CheckInResult.Sucessfull(checkIn.Id, checkIn.GymUserId, checkIn.TimeStamp);
        }

        public async Task<PageResult<CheckInGetResult>> GetCheckInsByDate(DateTime date, string searchString, int page, int pageSize, SortOrder sortOrder)
        {
            page -= 1;
            if (page <= 0)
                page = 0;

            var query = _dbContext.CheckInHistoryView.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString ?? "") && x.CheckInDate.Date == date.Date);

            if (sortOrder == SortOrder.Ascending || sortOrder == SortOrder.Unspecified)
                query = query.OrderBy(x => x.FirstName);
            else
                query = query.OrderByDescending(x => x.FirstName);

            var list = query.Skip(page * pageSize).Take(pageSize).ToList();

            return new PageResult<CheckInGetResult>
            {
                Count = query.ToList().Count,
                PageIndex = page,
                PageSize = pageSize,
                ActiveCount = 0,
                Items = list.Select(x => new CheckInGetResult()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email == "email" ? "null" : x.Email,
                    CheckInDate = x.CheckInDate,
                    GymUserId = x.GymUserId
                }).ToList()
            };
        }
    }
}