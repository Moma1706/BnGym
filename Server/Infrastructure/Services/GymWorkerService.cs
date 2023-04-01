using System;
using System.Net;
using Application.Common.Interfaces;
using Application.Common.Models.GymWorker;
using Application.Enums;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class GymWorkerService : IGymWorkerService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly string password = "BnGym2010";

        public GymWorkerService(IDateTimeService dateTimeService, ApplicationDbContext applicationDbContext, IIdentityService identityService)
        {
            _dateTimeService = dateTimeService;
            _dbContext = applicationDbContext;
            _identityService = identityService;
        }

        public async Task<GymWorkerResult> Create(string firstName, string lastName, string email)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                // crete user
                var result = await _identityService.Register(email, password, firstName, lastName, null);

                if (!result.Success)
                    return GymWorkerResult.Failure("Error while adding new user");

                // create gymWorker
                var gymWorker = new GymWorker
                {
                    UserId = result.Id
                };
                _dbContext.Add(gymWorker);

                // set role
                var userRoles = new IdentityUserRole<int>
                {
                    UserId = result.Id,
                    RoleId = (int)UserRole.Worker
                };
                _dbContext.Add(userRoles);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                return GymWorkerResult.Sucessfull();
            }
            catch (Exception)
            {
                transaction.Rollback();
                return GymWorkerResult.Failure("Fail to save gym worker");
            }
        }

        public async Task<GymWorkerResult> Delete(Guid id)
        {
            var gymWorker = await _dbContext.GymWorkers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymWorker == null)
                throw new KeyNotFoundException("Gym worker with provided id does not exist");

            var user = await _dbContext.Users.Where(x => x.Id == gymWorker.UserId).FirstOrDefaultAsync();
            if (user == null)
                return GymWorkerResult.Failure("User with provided id does not exist");

            user.IsBlocked = true;
            try
            {
                _dbContext.Update(user);
                return GymWorkerResult.Sucessfull();
            } catch (Exception)
            {
                return GymWorkerResult.Failure("Unable to delete gym worker");
            }
        }

        public async Task<IList<GymWorkerGetResult>> GetAll()// dodati paginaciju, sortiranje i filtriranje
        {
            var gymWorkerList = new List<GymWorkerGetResult>();

            var gymWorkers = await _dbContext.GymWorkers.ToListAsync();
            if (gymWorkers.Count == 0)
                return gymWorkerList;

            gymWorkerList = gymWorkers.Select(x => new GymWorkerGetResult()
            {
                Success = true,
                Error = string.Empty,
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                RoleId = x.RoleId,
            }).ToList();

            return gymWorkerList;
        }

        public async Task<GymWorkerGetResult> GetOne(Guid id)
        {
            var gymWorker = await _dbContext.GymWorkers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymWorker == null)
                throw new KeyNotFoundException("Gym worker with provided id does not exist");

            return GymWorkerGetResult.Sucessfull(gymWorker.Id, gymWorker.UserId, gymWorker.FirstName, gymWorker.LastName, gymWorker.Email, gymWorker.RoleId);
        }

        public Task<GymWorkerResult> Update(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

