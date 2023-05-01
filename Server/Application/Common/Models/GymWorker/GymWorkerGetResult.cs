using System;
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
        public bool IsBlocked { get; set; }
        public int RoleId { get; set; }

        public bool Success { get; set; }
        public Error Error { get; set; }

        public GymWorkerGetResult(bool success, Error error, Guid id, int userId, string firstName, string lastName, string email, int roleId, bool isBlocked)
        {
            Success = success;
            Error = error;
            Id = id;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            RoleId = roleId;
            IsBlocked = isBlocked;
        }

        public GymWorkerGetResult() { }

        public static GymWorkerGetResult Sucessfull(Guid id, int userId, string firstName, string lastName, string email, int roleId, bool isBlocked) => new(true, new Error { Code = 0, Message = string.Empty }, id, userId, firstName, lastName, email, roleId, isBlocked);
        public static GymWorkerGetResult Failure(Error error) => new(false, error, Guid.Empty, 0, string.Empty, string.Empty, string.Empty, 0, false);
    }
}
