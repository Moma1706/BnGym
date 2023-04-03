﻿using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;

namespace Application.Common.Models.GymWorker
{
	public class GymWorkerGetResult
	{
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }

        public bool Success { get; set; }
        public string Error { get; set; }

        public GymWorkerGetResult(bool success, string error, Guid id, int userId, string firstName, string lastName, string email, int roleId)
        {
            Success = success;
            Error = error;
            Id = id;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            RoleId = roleId;
        }

        public GymWorkerGetResult() {}

        public static GymWorkerGetResult Sucessfull(Guid id, int userId, string firstName, string lastName, string email, int roleId) => new(true, string.Empty, id, userId, firstName, lastName, email, roleId);
        public static GymWorkerGetResult Failure(string error) => new(false, error, Guid.Empty, 0, string.Empty, string.Empty, string.Empty, 0);
    }
}
