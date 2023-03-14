using System;
using System.Linq.Expressions;
using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.CheckIn;
using Application.Common.Models.GymUser;
using Application.Enums;
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
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                // crete user
                var result = await _identityService.Register(email, password, firstName, lastName, address);

                if (!result.Success)
                    return GymUserResult.Failure("Error while adding new user");

                // create gymUser
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

                var gymUser = new GymUser
                {
                    UserId = result.Id,
                    IsStudent = isStudent,
                    Type = type,
                    ExpiresOn = expiresOn
                };
                _dbContext.Add(gymUser);
                
                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                return GymUserResult.Sucessfull();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }

        public Task<GymUserResult> ActivateMembership(Guid id)
        {
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

        public async Task<IList<GymUserGetResult>> GetAll()
        {
            // dodati paginaciju, sortiranje i filtriranje
            var gymUsers = await _dbContext.GymUsers.ToListAsync();

            var userIds = gymUsers.Select(x => x.UserId);
            var users = _dbContext.Users.Where(u => userIds.Contains(u.Id)).ToList();

            var userList = new List<GymUserGetResult>();
            foreach (var gymUser in gymUsers)
            {
                var user = users.Where(x => x.Id == gymUser.UserId).FirstOrDefault();
                var gu = new GymUserGetResult
                {
                    Id = gymUser.Id,
                    ExpiresOn = gymUser.ExpiresOn,
                    IsStudent = gymUser.IsStudent,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
                userList.Add(gu);
            }

            return userList;
        }

        public async Task<GymUserGetResult> GetOne(Guid id)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            var user = await _dbContext.Users.Where(x => x.Id == gymUser.UserId).FirstOrDefaultAsync();

            var gu = new GymUserGetResult
            {
                Id = gymUser.Id,
                ExpiresOn = gymUser.ExpiresOn,
                IsStudent = gymUser.IsStudent,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };

            return gu;

        }

        public Task<GymUserResult> Update(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

