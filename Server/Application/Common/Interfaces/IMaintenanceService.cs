using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.DailyTraining;
using Application.Common.Models.Maintenance;
using Application.DailyTraining.Dtos;

namespace Application.Common.Interfaces
{
	public interface IMaintenanceService
	{
        Task<MaintenanceResult> CheckExpirationDate();
        Task<MaintenanceResult> CheckExpirationDate(Guid id);
        Task<MaintenanceResult> ClearCheckIns();
    }
}

