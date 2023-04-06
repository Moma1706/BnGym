using System;
using System.Linq.Expressions;
using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.CheckIn;
using Application.Common.Models.GymUser;
using Application.Common.Models.GymWorker;
using Application.Enums;
using Application.GymUser;
using Application.GymUser.Dtos;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
	public class GymUserService : IGymUserService
	{
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly string password = "BnGym2010";

        public GymUserService(IDateTimeService dateTimeService, ApplicationDbContext applicationDbContext, IIdentityService identityService)
		{
            _dateTimeService = dateTimeService;
            _dbContext = applicationDbContext;
            _identityService = identityService;
		}

        public async Task<GymUserResult> Create(string firstName, string lastName, string email, string address, GymUserType type)
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
                var result = await _identityService.Register(email, password, firstName, lastName, address);

                if (!result.Success)
                    return GymUserResult.Failure("Error while adding new user");

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
                    RoleId = (int)UserRole.RegularUser
                };
                _dbContext.Add(userRoles);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                return GymUserResult.Sucessfull();
            }
            catch (Exception)
            {
                transaction.Rollback();
                return GymUserResult.Failure("Fail to save gym user");
            }
        }

        public async Task<GymUserResult> ActivateMembership(Guid id)
        {
            // samo aktiviramo, pa on neka se cekira
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                return GymUserResult.Failure("Gym user with provided id does not exist");

            if (gymUser.IsInActive)
                return GymUserResult.Failure("Gym user is inactive");

            if (!gymUser.IsFrozen)
                return GymUserResult.Failure("Gym user has not a frozen membership");

            gymUser.IsFrozen = false;
            //gymUser.FreezeDate = null;

            // izracunati koliko dana mu je ostalo
            var days = (gymUser.ExpiresOn.Date - gymUser.FreezeDate.Date).Days;
            gymUser.ExpiresOn = gymUser.ExpiresOn.AddDays(days);
            gymUser.IsInActive = false;

            _dbContext.Update(gymUser);
            await _dbContext.SaveChangesAsync();

            return GymUserResult.Sucessfull();
        }

        public async Task<GymUserResult> Delete(Guid id)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                return GymUserResult.Failure("Gym user with provided id does not exist");

            var user = await _dbContext.Users.Where(x => x.Id == gymUser.UserId).FirstOrDefaultAsync();
            if (gymUser == null)
                return GymUserResult.Failure("User does not exist");

            gymUser.IsInActive = true;
            gymUser.ExpiresOn = _dateTimeService.Now;
            user.IsBlocked = true;

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Update(gymUser);
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
                return GymUserResult.Sucessfull();
            } catch (Exception)
            {
                transaction.Rollback();
                return GymUserResult.Failure("Fail to save gym user");
            }
        }

        public async Task<GymUserResult> ExtendMembership(Guid id, ExtendMembershipDto data)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (gymUser == null)
                return GymUserResult.Failure("Gym user with provided id does not exist");

            if (gymUser.IsFrozen)
                return GymUserResult.Failure("Gym user already has a frozen membership");

            if (gymUser.IsInActive)
                return GymUserResult.Failure("Gym user is inactive");

            if (gymUser.ExpiresOn <= _dateTimeService.Now)
                return GymUserResult.Failure("Gym user's membership has expired");

            var expiresOn = _dateTimeService.Now;
            if (gymUser.ExpiresOn > _dateTimeService.Now)
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

            gymUser.Type = data.Type;
            _dbContext.Update(gymUser);
            await _dbContext.SaveChangesAsync();
            return GymUserResult.Sucessfull();
        }

        public async Task<GymUserResult> FreezMembership(Guid id)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                return GymUserResult.Failure("Gym user with provided id does not exist");

            if (gymUser.IsFrozen)
                return GymUserResult.Failure("Gym user already has a frozen membership");

            if (gymUser.IsInActive)
                return GymUserResult.Failure("Gym user is inactive");


            if (gymUser.ExpiresOn <= _dateTimeService.Now)
                return GymUserResult.Failure("Gym user's membership has expired");

            gymUser.IsFrozen = true;
            gymUser.FreezeDate = _dateTimeService.Now;

            await _dbContext.SaveChangesAsync();
            return GymUserResult.Sucessfull();

        }

        public async Task<PageResult<GymUserGetResult>> GetAll(string searchString, int page, int pageSize)
        {
            var gymUserList = new List<GymUserGetResult>();

            // prepare result
            var countDetails = _dbContext.GymUserView.Count();
            var result = new PageResult<GymUserGetResult>
            {
                Count = countDetails,
                PageIndex = page,
                PageSize = pageSize,
                Items = gymUserList
            };

            if (page - 1 <= 0)
                page = 0;

            var query = _dbContext.GymUserView.Skip(page * pageSize).Take(pageSize);

            // applay searching string
            if (!String.IsNullOrEmpty(searchString))
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString));

            var gymUsers = await query.OrderBy(x => x.ExpiresOn).ToListAsync();
            if (gymUsers.Count == 0)
                return result;

            gymUserList = gymUsers.Select(x => new GymUserGetResult()
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                ExpiresOn = x.ExpiresOn,
                IsBlocked = x.IsBlocked,
                IsFrozen = x.IsFrozen,
                FreezeDate = x.FreezeDate,
                IsInactive = x.IsInactive,
                LastCheckIn = x.LastCheckIn,
                Type = x.Type,
                NumberOfArrivals = x.NumberOfArrivals,
                Address = x.Address,
            }).ToList();

            result.Items = gymUserList;
            return result;
        }

        public async Task<GymUserGetResult> GetOne(Guid id)
        {
            var gymUser = await _dbContext.GymUserView.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                throw new KeyNotFoundException("Gym user with provided id does not exist");

            return GymUserGetResult.Sucessfull(gymUser.Id, gymUser.UserId, gymUser.FirstName, gymUser.LastName, gymUser.Email, gymUser.ExpiresOn, gymUser.IsBlocked, gymUser.IsFrozen, gymUser.FreezeDate, gymUser.IsInactive, gymUser.LastCheckIn, gymUser.Type, gymUser.NumberOfArrivals, gymUser.Address);

        }

        public async Task<GymUserResult> Update(Guid id, UpdateGymUserDto data)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                GymUserResult.Failure("Gym user with provided id does not exist");

            var user = await _dbContext.Users.Where(x => x.Id == gymUser.UserId).FirstOrDefaultAsync();
            if (user == null)
                return GymUserResult.Failure("User does not exist");

            user.Email = data.Email ?? user.Email;
            user.FirstName = data.FirstName ?? user.FirstName;
            user.LastName = data.LastName ?? user.LastName;
            user.Address = data.Address ?? user.Address;

            //gymUser.Type = data.Type ?? ;
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                _dbContext.Update(gymUser);
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();

                transaction.Commit();
                return GymUserResult.Sucessfull();
            }
            catch (Exception)
            {
                transaction.Rollback();
                return GymUserResult.Failure("Fail to update gym user");
            }
        }
    }
}

