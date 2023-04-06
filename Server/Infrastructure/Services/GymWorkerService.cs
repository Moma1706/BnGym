using System;
using System.Net;
using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymWorker;
using Application.Enums;
using Application.GymWorker.Dtos;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

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
                    return GymWorkerResult.Failure("Error while adding new user ${result.Errors}");

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

        public async Task<PageResult<GymWorkerGetResult>> GetAll(string searchString, int page, int pageSize)
        {
            var gymWorkerList = new List<GymWorkerGetResult>();

            // prepare result
            var countDetails = _dbContext.GymWorkers.Count();
            var result = new PageResult<GymWorkerGetResult>
            {
                Count = countDetails,
                PageIndex = page,
                PageSize = pageSize,
                Items = gymWorkerList
            };

            if (page - 1 <= 0)
                page = 0;

            var query = _dbContext.GymWorkers.Skip(page * pageSize).Take(pageSize);
            // applay searching string
            if (!String.IsNullOrEmpty(searchString))
                query = query.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString));

            var gymWorkers = await query.OrderBy(x => x.FirstName).ToListAsync();
            if (gymWorkers.Count == 0)
                return result;

            gymWorkerList = gymWorkers.Select(x => new GymWorkerGetResult()
            {
                Id = x.Id,
                UserId = x.UserId,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                RoleId = x.RoleId,
            }).ToList();

            result.Items = gymWorkerList;
            return result;
        }

        public async Task<GymWorkerGetResult> GetOne(Guid id)
        {
            var gymWorker = await _dbContext.GymWorkers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymWorker == null)
                throw new KeyNotFoundException("Gym worker with provided id does not exist");

            return GymWorkerGetResult.Sucessfull(gymWorker.Id, gymWorker.UserId, gymWorker.FirstName, gymWorker.LastName, gymWorker.Email, gymWorker.RoleId);
        }

        public async Task<GymWorkerResult> Update(Guid id, UpdateGymWorkerDto data)
        {
            var gymWorker = await _dbContext.GymWorkers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymWorker == null)
                GymWorkerResult.Failure("Gym worker with provided id does not exist");

            var user = await _dbContext.Users.Where(x => x.Id == gymWorker.UserId).FirstOrDefaultAsync();
            if (user == null)
                return GymWorkerResult.Failure("User does not exist");

            user.Email = data.Email ?? user.Email;
            user.FirstName = data.FirstName ?? user.FirstName;
            user.LastName = data.LastName ?? user.LastName;

            _dbContext.Update(user);
            _dbContext.SaveChanges();
            return GymWorkerResult.Sucessfull();
        }
    }
}

