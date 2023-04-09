using Application.Common.Interfaces;
using Application.Common.Models.Maintenance;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    internal class MaintenanceService : IMaintenanceService
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ApplicationDbContext _dbContext;

        public MaintenanceService(ApplicationDbContext dbContext, IDateTimeService dateTimeService, UserManager<User> userManager)
        {
            _dateTimeService = dateTimeService;
            _dbContext = dbContext;
        }

        public async Task<MaintenanceResult> CheckExpirationDate()
        {
            var usersForUpdate = await _dbContext.GymUsers.Where(x => x.ExpiresOn.Date < _dateTimeService.Now.Date && x.IsInActive == false).ToListAsync();
            if (usersForUpdate.Count() == 0)
                return MaintenanceResult.Sucessfull();

            try
            {
                foreach (GymUser user in usersForUpdate)
                {
                    user.IsInActive = true;
                }

                _dbContext.GymUsers.UpdateRange(usersForUpdate);
                await _dbContext.SaveChangesAsync();
                return MaintenanceResult.Sucessfull();
            } catch (Exception)
            {
                return MaintenanceResult.Failure("Unable to update gym users");
            }
        }

        public async Task<MaintenanceResult> CheckExpirationDate(Guid id)
        {
            var gymUser = await _dbContext.GymUsers.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (gymUser == null)
                return MaintenanceResult.Failure("Gym user does not exist");

            if (gymUser.ExpiresOn.Date < _dateTimeService.Now.Date && gymUser.IsInActive == false)
            {
                gymUser.IsInActive = true;
                _dbContext.Update(gymUser);
                _dbContext.SaveChanges();
                return MaintenanceResult.Sucessfull();
            }
            return MaintenanceResult.Sucessfull();
        }

        public async Task<MaintenanceResult> ClearCheckIns()
        {
            var currentDate = _dateTimeService.Now.Date;
            var dateForDelete = currentDate.AddMonths(-1);

            try {
                var data = await _dbContext.CheckIns.Where(x => x.TimeStamp.Date < dateForDelete.Date).ToListAsync();
                if (data.Count() == 0)
                    return MaintenanceResult.Sucessfull();

                _dbContext.CheckIns.RemoveRange(data);
                await _dbContext.SaveChangesAsync();
                return MaintenanceResult.Sucessfull();
            } catch (Exception)
            {
                return MaintenanceResult.Failure("Unable to clear check ins data");
            }
        }
    }
}
