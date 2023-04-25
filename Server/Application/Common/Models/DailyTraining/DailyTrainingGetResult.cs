using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;

namespace Application.Common.Models.DailyTraining
{
    public class DailyTrainingGetResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastCheckIn { get; set; }

        public bool Success { get; set; }
        public Error Error { get; set; }

        public DailyTrainingGetResult(bool success, Error error, Guid id, string firstName, string lastName, DateTime dateOfBirth, DateTime lastCheckIn)
        {
            Success = success;
            Error = error;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            LastCheckIn = lastCheckIn;
        }

        public DailyTrainingGetResult() { }

        public static DailyTrainingGetResult Sucessfull(Guid id, string firstName, string lastName, DateTime dateOfBirth, DateTime lastCheckIn) => new(true, new Error { Code = 0, Message = string.Empty}, id, firstName, lastName, dateOfBirth, lastCheckIn);
        public static DailyTrainingGetResult Failure(Error error) => new(false, error, Guid.Empty, string.Empty, string.Empty, DateTime.Now, DateTime.Now);
    }
}
