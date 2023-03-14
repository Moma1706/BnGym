using System;
using Application.Common.Models.Auth;
using Application.Common.Models.GymUser;
using Application.Enums;

namespace Application.Common.Interfaces
{
	public interface IGymUserService
	{
        Task<GymUserResult> Create(string firstName, string lastName, string email, string address, bool isStudent, GymUserType type);

        Task<IList<GymUserGetResult>> GetAll();

        Task<GymUserGetResult> GetOne(Guid id);

        Task<GymUserResult> Delete(Guid id);

        Task<GymUserResult> Update(Guid id);

        Task<GymUserResult> ExtendMembership(Guid id, GymUserType type);

        Task<GymUserResult> FreezMembership(Guid id);

        Task<GymUserResult> ActivateMembership(Guid id);
    }
}

