using System;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;

namespace Application.Common.Models.DailyUser
{
    public class DailyUserGetResult
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastCheckIn { get; set; }
        public int NumberOfArrivalsCurrentMonth { get; set; }
        public int NumberOfArrivalsLastMonth { get; set; }

        public bool Success { get; set; }
        public Error Error { get; set; }

        public DailyUserGetResult(bool success, Error error, Guid id, string firstName, string lastName, DateTime dateOfBirth, DateTime lastCheckIn, int numberOfArrivalsCurrentMonth, int numberOfArrivalsLastMonth)
        {
            Success = success;
            Error = error;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            LastCheckIn = lastCheckIn;
            NumberOfArrivalsCurrentMonth = numberOfArrivalsCurrentMonth;
            NumberOfArrivalsLastMonth = numberOfArrivalsLastMonth;

        }

        public DailyUserGetResult() { }
        public DailyUserGetResult(Error error)
        {
            Error = error;
        }

        public static DailyUserGetResult Sucessfull(Guid id, string firstName, string lastName, DateTime dateOfBirth, DateTime lastCheckIn, int numberOfArrivalsCurrentMonth, int numberOfArrivalsLastMonth) => new(true, new Error { Code = 0, Message = string.Empty}, id, firstName, lastName, dateOfBirth, lastCheckIn, numberOfArrivalsCurrentMonth, numberOfArrivalsLastMonth);
        public static DailyUserGetResult Failure(Error error) => new(error);
    }
}
