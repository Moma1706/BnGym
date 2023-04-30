using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymWorker;
using Application.Common.Models.User;
using Application.GymWorker.Dtos;

namespace Application.Common.Interfaces
{
	public interface IGymWorkerService
	{
        Task<GymWorkerGetResult> Create(string firstName, string lastName, string email);

        Task<PageResult<GymWorkerGetResult>> GetAll(string searchString, int page, int pageSize);

        Task<GymWorkerGetResult> GetOne(Guid id);

        Task<GymWorkerResult> Delete(Guid id);

        Task<GymWorkerResult> Update(int id, UpdateGymWorkerDto data);

        Task<GymWorkerResult> Activate(Guid id);

        Task<UserGetResult> GetUser(int id);
    }
}

