﻿using System;
using Application.App.Dtos;
using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;
using Application.Enums;
using Application.GymUser;
using Application.GymUser.Dtos;

namespace Application.Common.Interfaces
{
	public interface IGymUserService
	{
        Task<GymUserGetResult> Create(string firstName, string lastName, string email, string address, GymUserType type);

        Task<PageResult<GymUserGetResult>> GetAll(string searchString, int page, int pageSize);

        Task<GymUserGetResult> GetOne(int id);

        Task<GymUserResult> Update(Guid id, UpdateGymUserDto data);

        Task<GymUserResult> ExtendMembership(Guid id, ExtendMembershipDto data);

        Task<GymUserResult> FreezMembership(Guid id);

        Task<GymUserResult> ActivateMembership(Guid id);

        Task<GymUserResult> UpdateRegularUser(int id, UpdateRegularUserDto data);
    }
}
