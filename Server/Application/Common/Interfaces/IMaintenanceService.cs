using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyUser;
using Application.Common.Models.Maintenance;
using Application.DailyUser.Dtos;

namespace Application.Common.Interfaces
{
	public interface IMaintenanceService
	{
        Task<MaintenanceResult> CheckExpirationDate();
        Task<MaintenanceResult> CheckExpirationDate(int id);
        Task<MaintenanceResult> ClearCheckIns();
    }
}

