using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;

namespace Application.Common.Models.DailyTraining
{
    public class DailyUsersGetResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public bool Success { get; set; }
        public string Error { get; set; }

        public DailyUsersGetResult(bool success, string error, Guid id, string firstName, string lastName, DateTime dateOfBirth)
        {
            Success = success;
            Error = error;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }

        public DailyUsersGetResult() { }

        public static DailyUsersGetResult Sucessfull(Guid id, string firstName, string lastName, DateTime dateOfBirth) => new(true, string.Empty, id, firstName, lastName, dateOfBirth);
        public static DailyUsersGetResult Failure(string error) => new(false, error, Guid.Empty, string.Empty, string.Empty, DateTime.Now);
    }
}
