using System;
using Application.Common.Models.GymWorker;

namespace Application.Common.Interfaces
{
	public interface IGymWorkerService
	{
        Task<GymWorkerResult> Create(string firstName, string lastName, string email);

        Task<IList<GymWorkerGetResult>> GetAll();

        Task<GymWorkerGetResult> GetOne(Guid id);

        Task<GymWorkerResult> Delete(Guid id);

        Task<GymWorkerResult> Update(Guid id);
    }
}

