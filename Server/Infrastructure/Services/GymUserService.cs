﻿using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;
using Application.Enums;
using Application.GymUser.Dtos;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Application.App.Dtos;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Services
{
    public class GymUserService : IGymUserService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IIdentityService _identityService;
        private readonly IMaintenanceService _maintenanceService;

        public GymUserService(IDateTimeService dateTimeService, ApplicationDbContext applicationDbContext, IIdentityService identityService, IMaintenanceService maintenanceService, IEmailService emailService, UserManager<User> userManager)
        {
            _dateTimeService = dateTimeService;
            _dbContext = applicationDbContext;
            _identityService = identityService;
            _maintenanceService = maintenanceService;
            _userManager = userManager;
        }

        public async Task<GymUserGetResult> Create(string firstName, string lastName, string email, string address, GymUserType type)
        {
            var currentDate = _dateTimeService.Now;
            var expiresOn = _dateTimeService.Now;
            switch (type)
            {
                case GymUserType.HalfMonth:
                    expiresOn = currentDate.AddDays(15);
                    break;

                case GymUserType.Month:
                    expiresOn = currentDate.AddMonths(1);
                    break;

                case GymUserType.ThreeMonts:
                    expiresOn = currentDate.AddMonths(3);
                    break;

                case GymUserType.HalfYear:
                    expiresOn = currentDate.AddMonths(6);
                    break;

                case GymUserType.Year:
                    expiresOn = currentDate.AddYears(1);
                    break;
            }

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                // crete user
                var result = await _identityService.Register(email, firstName, lastName, address);

                if (!result.Success)
                    return GymUserGetResult.Failure(new Error { Code = ExceptionType.UnableToRegister, Message = result.Errors });

                // create gymUser
                var gymUser = new GymUser
                {
                    UserId = result.Id,
                    Type = type,
                    ExpiresOn = expiresOn
                };
                _dbContext.Add(gymUser);

                // set role
                var userRoles = new IdentityUserRole<int>
                {
                    UserId = result.Id,
                    RoleId = Convert.ToInt32(UserRole.RegularUser)
                };
                _dbContext.Add(userRoles);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                return GymUserGetResult.Sucessfull(
                    gymUser.Id,
                    gymUser.UserId,
                    firstName,
                    lastName,
                    email,
                    expiresOn,
                    false,
                    gymUser.IsFrozen,
                    gymUser.FreezeDate == DateTime.MinValue ? "null" : gymUser.FreezeDate.ToString(),
                    gymUser.IsInActive,
                    gymUser.LastCheckIn == DateTime.MinValue ? "null" : gymUser.LastCheckIn.ToString(),
                    gymUser.Type,
                    address,
                    0,
                    0);
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                return GymUserGetResult.Failure(new Error { Code = ExceptionType.UnableToCreate, Message = "Nije moguće sačuvati korisnika. " + exc.Message });
            }
        }

        public async Task<GymUserResult> ActivateMembership(Guid id)
        {
            // samo aktiviramo, pa on neka se cekira
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                return GymUserResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

            if (!gymUser.IsFrozen)
                return GymUserResult.Failure(new Error { Code = ExceptionType.UserIsFrozen, Message = "Korisnik je već aktivan" });

            // izracunati koliko dana mu je ostalo
            var currentDateTime = _dateTimeService.Now;
            var currentDate = currentDateTime.Date;
            var expiresOn = gymUser.ExpiresOn.Date;
            var days = 0;
            // clanarina istekla
            if (expiresOn < currentDate)
            {
                days = (expiresOn - gymUser.FreezeDate.Date).Days;
                gymUser.ExpiresOn = currentDateTime.AddDays(days);
            }
            else
            {
                days = (currentDate - gymUser.FreezeDate.Date).Days;
                gymUser.ExpiresOn = gymUser.ExpiresOn.AddDays(days);
            }

            gymUser.IsInActive = false;
            gymUser.FreezeDate = DateTime.MinValue;
            gymUser.IsFrozen = false;

            _dbContext.Update(gymUser);
            await _dbContext.SaveChangesAsync();

            return GymUserResult.Sucessfull();
        }

        public async Task<GymUserResult> ActivateAllMemberships()
        {
            // samo aktiviramo, pa on neka se cekira
            var gymUsers = await _dbContext.GymUsers.Where(x => x.IsFrozen == true).ToListAsync();
            if (gymUsers.Count == 0)
                return GymUserResult.Sucessfull();

            // izracunati koliko dana im je ostalo
            var currentDateTime = _dateTimeService.Now;
            var currentDate = currentDateTime.Date;
            foreach (GymUser gymUser in gymUsers)
            {
                var expiresOn = gymUser.ExpiresOn.Date;
                var days = 0;
                if (expiresOn < currentDate)
                {
                    days = (expiresOn - gymUser.FreezeDate.Date).Days;
                    gymUser.ExpiresOn = currentDateTime.AddDays(days);
                }
                else
                {
                    days = (currentDate - gymUser.FreezeDate.Date).Days;
                    gymUser.ExpiresOn = gymUser.ExpiresOn.AddDays(days);
                }

                gymUser.FreezeDate = DateTime.MinValue;
                gymUser.IsFrozen = false;
                gymUser.IsInActive = false;
            }

            _dbContext.GymUsers.UpdateRange(gymUsers);
            await _dbContext.SaveChangesAsync();

            return GymUserResult.Sucessfull();
        }

        public async Task<GymUserResult> ExtendMembership(Guid id, ExtendMembershipDto data)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (gymUser == null)
                return GymUserResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

            if (gymUser.IsFrozen)
                return GymUserResult.Failure(new Error { Code = ExceptionType.UserIsFrozen, Message = "Nije moguće produžiti članarinu. Korisnikov status: ZALEDJEN" });

            var expiresOn = _dateTimeService.Now;
            if (gymUser.ExpiresOn > expiresOn)
                expiresOn = gymUser.ExpiresOn;

            switch (data.Type)
            {
                case GymUserType.HalfMonth:
                    expiresOn = expiresOn.AddDays(15);
                    break;

                case GymUserType.Month:
                    expiresOn = expiresOn.AddMonths(1);
                    break;

                case GymUserType.ThreeMonts:
                    expiresOn = expiresOn.AddMonths(3);
                    break;

                case GymUserType.HalfYear:
                    expiresOn = expiresOn.AddMonths(6);
                    break;

                case GymUserType.Year:
                    expiresOn = expiresOn.AddYears(1);
                    break;
            }

            gymUser.ExpiresOn = expiresOn;
            gymUser.Type = data.Type;
            gymUser.IsInActive = false;

            _dbContext.Update(gymUser);
            await _dbContext.SaveChangesAsync();
            return GymUserResult.Sucessfull();
        }

        public async Task<GymUserResult> FreezMembership(Guid id)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                return GymUserResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

            // Check expiration date
            var maintenanceResult = await _maintenanceService.CheckExpirationDate(gymUser.UserId);
            if (gymUser.IsFrozen)
                return GymUserResult.Failure(new Error { Code = ExceptionType.UserIsFrozen, Message = "Korisnik je već zaledjen" });

            if (gymUser.IsInActive)
                return GymUserResult.Failure(new Error { Code = ExceptionType.UserIsInActive, Message = "Korisnikova članarina je istekla" });

            if (gymUser.ExpiresOn < _dateTimeService.Now)
                return GymUserResult.Failure(new Error { Code = ExceptionType.ExpiredMembership, Message = "Korisnikova članarina je istekla" });

            gymUser.IsFrozen = true;
            gymUser.FreezeDate = _dateTimeService.Now;

            _dbContext.Update(gymUser);
            await _dbContext.SaveChangesAsync();
            return GymUserResult.Sucessfull();
        }

        public async Task<GymUserResult> FreezAllMemberships()
        {
            // Check expiration date
            var maintenanceResult = await _maintenanceService.CheckExpirationDate();

            var gymUsers = await _dbContext.GymUsers.Where(x => x.IsFrozen == false && x.IsInActive == false && x.ExpiresOn > _dateTimeService.Now).ToListAsync();
            if (gymUsers.Count == 0)
                return GymUserResult.Sucessfull();

            foreach (GymUser gymUser in gymUsers)
            {
                gymUser.IsFrozen = true; // Set isFrozen to true
                gymUser.FreezeDate = _dateTimeService.Now; ; // Set FrozenDate to current date and time
            }

            _dbContext.GymUsers.UpdateRange(gymUsers);
            await _dbContext.SaveChangesAsync();

            return GymUserResult.Sucessfull();
        }

        public async Task<PageResult<GymUserGetResult>> GetAll(string searchString, int page, int pageSize, SortOrder sortOrder, string sortParam = "")
        {
            page -= 1;
            if (page <= 0)
                page = 0;
            var query = _dbContext.GymUserView.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString ?? ""));

            if (sortParam == "NumberOfArrivalsLastMonth")
            {
                if (sortOrder == SortOrder.Ascending || sortOrder == SortOrder.Unspecified)
                    query = query.OrderBy(x => x.NumberOfArrivalsLastMonth);
                else
                    query = query.OrderByDescending(x => x.NumberOfArrivalsLastMonth);
            }
            else if (sortParam == "NumberOfArrivalsCurrentMonth")
            {
                if (sortOrder == SortOrder.Ascending || sortOrder == SortOrder.Unspecified)
                    query = query.OrderBy(x => x.NumberOfArrivalsCurrentMonth);
                else
                    query = query.OrderByDescending(x => x.NumberOfArrivalsCurrentMonth);
            }
            else
            {
                if (sortOrder == SortOrder.Ascending || sortOrder == SortOrder.Unspecified)
                    query = query.OrderBy(x => x.ExpiresOn)
                        .ThenByDescending(x => x.NumberOfArrivalsCurrentMonth)
                        .ThenByDescending(x => x.NumberOfArrivalsLastMonth);
                else
                    query = query.OrderByDescending(x => x.ExpiresOn)
                        .ThenByDescending(x => x.NumberOfArrivalsCurrentMonth)
                        .ThenByDescending(x => x.NumberOfArrivalsLastMonth);
            }

            var list = query.ToList().Skip(page * pageSize).Take(pageSize).ToList();

            return new PageResult<GymUserGetResult>
            {
                Count = query.ToList().Count,
                PageIndex = page,
                PageSize = pageSize,
                ActiveCount = _dbContext.GymUserView.Where(x => x.IsInActive == false).Count(),
                Items = list.Select(x => new GymUserGetResult()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    ExpiresOn = x.ExpiresOn,
                    IsBlocked = x.IsBlocked,
                    IsFrozen = x.IsFrozen,
                    FreezeDate = x.FreezeDate == DateTime.MinValue ? "null" : x.FreezeDate.ToString(),
                    IsInActive = x.IsInActive,
                    LastCheckIn = x.LastCheckIn == DateTime.MinValue ? "null" : x.LastCheckIn.ToString(),
                    Type = x.Type,
                    Address = x.Address,
                    NumberOfArrivalsCurrentMonth = x.NumberOfArrivalsCurrentMonth,
                    NumberOfArrivalsLastMonth = x.NumberOfArrivalsLastMonth
                }).ToList()
            };
        }

        public async Task<GymUserGetResult> GetOne(int id)
        {
            var gymUser = await _dbContext.GymUserView.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (gymUser == null)
                return GymUserGetResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

            // Check expiration date
            var maintenanceResult = await _maintenanceService.CheckExpirationDate(id);

            return GymUserGetResult.Sucessfull(gymUser.Id, gymUser.UserId, gymUser.FirstName, gymUser.LastName, gymUser.Email, gymUser.ExpiresOn, gymUser.IsBlocked, gymUser.IsFrozen,
                gymUser.FreezeDate == DateTime.MinValue ? "null" : gymUser.FreezeDate.ToString(),
                gymUser.IsInActive,
                gymUser.LastCheckIn == DateTime.MinValue ? "null" : gymUser.LastCheckIn.ToString(),
                gymUser.Type, gymUser.Address,
                gymUser.NumberOfArrivalsLastMonth, gymUser.NumberOfArrivalsCurrentMonth);
        }

        public async Task<GymUserGetResult> GetRegularOne(Guid id)
        {
            var gymUser = await _dbContext.GymUserView.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                return GymUserGetResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

            // Check expiration date
            var maintenanceResult = await _maintenanceService.CheckExpirationDate(gymUser.UserId);

            return GymUserGetResult.Sucessfull(gymUser.Id, gymUser.UserId, gymUser.FirstName, gymUser.LastName, gymUser.Email, gymUser.ExpiresOn, gymUser.IsBlocked, gymUser.IsFrozen,
                gymUser.FreezeDate == DateTime.MinValue ? "null" : gymUser.FreezeDate.ToString(),
                gymUser.IsInActive,
                gymUser.LastCheckIn == DateTime.MinValue ? "null" : gymUser.LastCheckIn.ToString(),
                gymUser.Type, gymUser.Address,
                gymUser.NumberOfArrivalsLastMonth, gymUser.NumberOfArrivalsCurrentMonth);
        }

        public async Task<GymUserResult> Update(Guid id, UpdateGymUserDto data)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                return GymUserResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

            var user = await _dbContext.Users.Where(x => x.Id == gymUser.UserId).FirstOrDefaultAsync();
            if (user == null)
                return GymUserResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik ne postoji" });

            if (data.Email is not null && data.Email.ToLower() != user.Email)
            {
                var email = data.Email.ToLower();
                if (await _userManager.FindByEmailAsync(email) != null)
                    return GymUserResult.Failure(new Error { Code = ExceptionType.EmailAlredyExists, Message = "Korisnik sa navedenim email-om već postoji" });

                user.Email = email;
                user.UserName = email;
                user.EmailConfirmed = true;
            }

            user.FirstName = data.FirstName ?? user.FirstName;
            user.LastName = data.LastName ?? user.LastName;
            user.Address = data.Address ?? user.Address;

            // Check expiration date
            if (data.Type != gymUser.Type)
            {
                if (gymUser.ExpiresOn.Date < _dateTimeService.Now.Date && gymUser.IsInActive == false)
                {
                    gymUser.IsInActive = true;
                    _dbContext.Update(gymUser);
                    _dbContext.SaveChanges();
                    return GymUserResult.Failure(new Error { Code = ExceptionType.UnableToUpdate, Message = "Korisniku je istekla članarina" });
                }

                if (gymUser.IsInActive || gymUser.IsFrozen)
                    return GymUserResult.Failure(new Error { Code = ExceptionType.UnableToUpdate, Message = "Korisnik je zaledjen ili mu je istekla članarina" });

                // racunamo kada je user uplatio teretanu
                // od expires_on oduzmemo koji je tip uplacen
                var dateOfPayment = _dateTimeService.Now;
                var gymUserExpiresOn = gymUser.ExpiresOn;
                switch (gymUser.Type)
                {
                    case GymUserType.HalfMonth:
                        dateOfPayment = gymUserExpiresOn.AddDays(-15);
                        break;

                    case GymUserType.Month:
                        dateOfPayment = gymUserExpiresOn.AddMonths(-1);
                        break;

                    case GymUserType.ThreeMonts:
                        dateOfPayment = gymUserExpiresOn.AddMonths(-3);
                        break;

                    case GymUserType.HalfYear:
                        dateOfPayment = gymUserExpiresOn.AddMonths(-6);
                        break;

                    case GymUserType.Year:
                        dateOfPayment = gymUserExpiresOn.AddYears(-1);
                        break;
                }

                var expiresOn = _dateTimeService.Now;
                switch (data.Type)
                {
                    case GymUserType.HalfMonth:
                        gymUser.Type = GymUserType.HalfMonth;
                        expiresOn = dateOfPayment.AddDays(15);
                        break;

                    case GymUserType.Month:
                        gymUser.Type = GymUserType.Month;
                        expiresOn = dateOfPayment.AddMonths(1);
                        break;

                    case GymUserType.ThreeMonts:
                        gymUser.Type = GymUserType.ThreeMonts;
                        expiresOn = dateOfPayment.AddMonths(3);
                        break;

                    case GymUserType.HalfYear:
                        gymUser.Type = GymUserType.HalfYear;
                        expiresOn = dateOfPayment.AddMonths(6);
                        break;

                    case GymUserType.Year:
                        gymUser.Type = GymUserType.Year;
                        expiresOn = dateOfPayment.AddYears(1);
                        break;

                    default:
                        expiresOn = gymUser.ExpiresOn;
                        break;
                }

                gymUser.ExpiresOn = expiresOn;

                if (_dateTimeService.Now > expiresOn)
                    return GymUserResult.Failure(new Error { Code = ExceptionType.InvalidGymUserType, Message = "Nemoguće ažurirati korisnika. Nevalidan tip članarine" });
            }
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                var registerResult = await _userManager.UpdateAsync(user);

                if (!registerResult.Succeeded)
                    return GymUserResult.Failure(new Error { Code = ExceptionType.UnableToUpdate, Message = "Nemoguće ažurirati korisnika" });

                _dbContext.Update(gymUser);
                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                return GymUserResult.Sucessfull();
            }
            catch (Exception exc)
            {
                transaction.Rollback();
                return GymUserResult.Failure(new Error { Code = ExceptionType.UnableToUpdate, Message = "Nemoguće ažurirati korisnika. " + exc.Message });
            }
        }

        public async Task<GymUserResult> UpdateRegularUser(Guid id, UpdateRegularUserDto data)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                GymUserResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji" });

            var user = await _dbContext.Users.Where(x => x.Id == gymUser.UserId).FirstOrDefaultAsync();
            if (user == null)
                return GymUserResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik ne postoji" });

            if (data.Email is not null && data.Email.ToLower() != user.Email)
            {
                var email = data.Email.ToLower();
                if (await _userManager.FindByEmailAsync(email) != null)
                    return GymUserResult.Failure(new Error { Code = ExceptionType.EmailAlredyExists, Message = "Korisnik sa navedenim email-om već postoji" });

                user.Email = email;
                user.UserName = email;
                user.EmailConfirmed = true;
            }

            user.FirstName = data.FirstName ?? user.FirstName;
            user.LastName = data.LastName ?? user.LastName;
            user.Address = data.Address ?? user.Address;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
                return GymUserResult.Failure(new Error { Code = ExceptionType.UnableToUpdate, Message = "Nije moguće ažurirati korisnika" });

            return GymUserResult.Sucessfull();
        }
    }
}