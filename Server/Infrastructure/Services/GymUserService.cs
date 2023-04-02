using System;
using System.Linq.Expressions;
using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.CheckIn;
using Application.Common.Models.GymUser;
using Application.Common.Models.GymWorker;
using Application.Enums;
using Application.GymUser;
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


        public async Task<GymUserResult> Create(string firstName, string lastName, string email, string address, bool isStudent, GymUserType type)
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
                    IsStudent = isStudent,
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

        public Task<GymUserResult> ActivateMembership(Guid id)
        {
            // postaviti NumberOfArrivals na 1
            throw new NotImplementedException();
        }

        public Task<GymUserResult> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GymUserResult> ExtendMembership(Guid id, GymUserType type)
        {
            throw new NotImplementedException();
        }

        public Task<GymUserResult> FreezMembership(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<GymUserGetResult>> GetAll() // dodati paginaciju, sortiranje i filtriranje
        {
            var gymUserList = new List<GymUserGetResult>();

            var gymUsers = await _dbContext.GymUserView.ToListAsync();
            if (gymUsers.Count == 0)
                return gymUserList;

            gymUserList = gymUsers.Select(x => new GymUserGetResult()
            {
                Success = true,
                Error = string.Empty,
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                IsStudent = x.IsStudent,
                ExpiresOn = x.ExpiresOn,
                IsBlocked = x.IsBlocked,
                IsFrozen = x.IsFrozen,
                FreezeDate = x.FreezeDate,
                IsInactive = x.IsInactive,
                LastCheckIn = x.LastCheckIn,
                Type = x.Type,
                NumberOfArrivals = x.NumberOfArrivals
            }).ToList();

            return gymUserList;
        }

        public async Task<GymUserGetResult> GetOne(Guid id)
        {
            var gymUser = await _dbContext.GymUserView.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                throw new KeyNotFoundException("Gym worker with provided id does not exist");

            return GymUserGetResult.Sucessfull(gymUser.Id, gymUser.UserId, gymUser.FirstName, gymUser.LastName, gymUser.Email, gymUser.IsStudent, gymUser.ExpiresOn, gymUser.IsBlocked, gymUser.IsFrozen, gymUser.FreezeDate, gymUser.IsInactive, gymUser.LastCheckIn, gymUser.Type, gymUser.NumberOfArrivals);

        }

        public Task<GymUserResult> Update(Guid id, UpdateCommand data)
        {
            throw new NotImplementedException();
        }
    }
}

