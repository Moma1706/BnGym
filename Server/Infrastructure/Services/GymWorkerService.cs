using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Application.Common.Interfaces;
using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;
using Application.Common.Models.GymWorker;
using Application.Common.Models.User;
using Application.Enums;
using Application.GymWorker.Dtos;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Services
{
    public class GymWorkerService : IGymWorkerService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public GymWorkerService(IDateTimeService dateTimeService, ApplicationDbContext applicationDbContext, IIdentityService identityService, IEmailService emailService, UserManager<User> userManager)
        {
            _dateTimeService = dateTimeService;
            _dbContext = applicationDbContext;
            _identityService = identityService;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<GymWorkerGetResult> Create(string firstName, string lastName, string email)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                // crete user
                var result = await _identityService.Register(email, firstName, lastName, null);

                if (!result.Success)
                    return GymWorkerGetResult.Failure(new Error { Code = ExceptionType.UnableToRegister, Message = result.Errors });

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
                    RoleId = Convert.ToInt32(UserRole.Admin)
                };
                _dbContext.Add(userRoles);

                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                return GymWorkerGetResult.Sucessfull(gymWorker.Id, gymWorker.UserId, firstName, lastName, email, userRoles.RoleId, false);
            }
            catch (Exception)
            {
                transaction.Rollback();
                return GymWorkerGetResult.Failure(new Error { Code = ExceptionType.UnableToCreate, Message = "Fail to save gym worker" });
            }
        }

        public async Task<GymWorkerResult> Delete(Guid id)
        {
            var gymWorker = await _dbContext.GymWorkers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymWorker == null)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Gym worker with provided id does not exist" });

            var user = await _dbContext.Users.Where(x => x.Id == gymWorker.UserId).FirstOrDefaultAsync();
            if (user == null)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "User with provided id does not exist" });

            if (user.IsBlocked)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.UserIsBlocked, Message = "User with provided id is blocked" });

            user.IsBlocked = true;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.UnableToDelete, Message = "Unable to delete gym worker" });

            return GymWorkerResult.Sucessfull();
        }

        public async Task<PageResult<GymWorkerGetResult>> GetAll(string searchString, int page, int pageSize, SortOrder sortOrder)
        {
            var gymWorkerList = new List<GymWorkerGetResult>();

            page -= 1;
            if (page <= 0)
                page = 0;

            //if you perform first pagination then sorting then you are missing data (search-sort-pagination this is the order)
            var gymWorkers = _dbContext.GymWorkers.Where(x => (x.FirstName + " " + x.LastName).Contains(searchString ?? "")); //first apply filter
            if (sortOrder == SortOrder.Ascending || sortOrder == SortOrder.Unspecified)
                gymWorkers = gymWorkers.OrderBy(x => x.FirstName).ThenBy(x => x.LastName);
            else
                gymWorkers = gymWorkers.OrderByDescending(x => x.FirstName).ThenByDescending(x => x.LastName);

            var gymWorkersList = gymWorkers.ToList().Skip(page * pageSize).Take(pageSize).ToList(); //then pagination

            return new PageResult<GymWorkerGetResult>
            {
                Count = gymWorkers.ToList().Count,
                PageIndex = page,
                PageSize = pageSize,
                Items = gymWorkersList.Select(x => new GymWorkerGetResult()
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    RoleId = x.RoleId,
                    IsBlocked = x.IsBlocked
                }).ToList()
            };
        }

        public async Task<GymWorkerGetResult> GetOne(Guid id)
        {
            var gymWorker = await _dbContext.GymWorkers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymWorker == null)
                return GymWorkerGetResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Gym worker with provided id does not exist" });

            return GymWorkerGetResult.Sucessfull(gymWorker.Id, gymWorker.UserId, gymWorker.FirstName, gymWorker.LastName, gymWorker.Email, gymWorker.RoleId, gymWorker.IsBlocked);
        }

        public async Task<GymWorkerResult> Update(int id, UpdateGymWorkerDto data)
        {
            var sendMail = false;
            var user = await _dbContext.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Korisnik sa proslijedjenim id ne postoji!" });

            if (data.Email is string && data.Email.ToLower() != user.Email)
            {
                var email = data.Email.ToLower();
                if (await _userManager.FindByEmailAsync(email) != null)
                    return GymWorkerResult.Failure(new Error { Code = ExceptionType.EmailAlredyExists, Message = "Korisnik sa navedenim emailom već postoji!" });

                user.Email = email;
                user.UserName = email;
                user.EmailConfirmed = true;
                sendMail = true;
            }

            user.FirstName = data.FirstName ?? user.FirstName;
            user.LastName = data.LastName ?? user.LastName;

            var registerResult = await _userManager.UpdateAsync(user);

            if (!registerResult.Succeeded)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.UnableToUpdate, Message = "Nije moguće ažurirati korisnika. " + registerResult.Errors });

            if (sendMail)
                _emailService.SendConfirmationEmailAsync(user.Email);
            return GymWorkerResult.Sucessfull();
        }

        public async Task<GymWorkerResult> Activate(Guid id)
        {
            var gymWorker = await _dbContext.GymWorkers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymWorker == null)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "Gym worker with provided id does not exist" });

            var user = await _dbContext.Users.Where(x => x.Id == gymWorker.UserId).FirstOrDefaultAsync();
            if (user == null)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "User does not exist" });

            if (!user.IsBlocked)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.WorkerIsActive, Message = "Gym worker is active" });

            user.IsBlocked = false;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return GymWorkerResult.Failure(new Error { Code = ExceptionType.UnableToDelete, Message = "Unable to delete gym worker" });

            return GymWorkerResult.Sucessfull();
        }

        public async Task<UserGetResult> GetUser(int Id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            if (user == null)
                return UserGetResult.Failure(new Error { Code = ExceptionType.EntityNotExist, Message = "User with provided id does not exist" });

            return UserGetResult.Sucessfull(user.Id, user.FirstName, user.LastName, user.Email);
        }
    }
}