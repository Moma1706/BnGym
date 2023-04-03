using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymWorker;

namespace Application.Common.Interfaces
{
	public interface IGymWorkerService
	{
        Task<GymWorkerResult> Create(string firstName, string lastName, string email);

        Task<PageResult<GymWorkerGetResult>> GetAll(string searchString, int page, int pageSize);

        Task<GymWorkerGetResult> GetOne(Guid id);

        Task<GymWorkerResult> Delete(Guid id);

        Task<GymWorkerResult> Update(Guid id);
    }
}

