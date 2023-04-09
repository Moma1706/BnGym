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
        public int NumberOfArrivals { get; set; }
        public DateTime CheckInDate { get; set; }

        public bool Success { get; set; }
        public string Error { get; set; }

        public DailyTrainingGetResult(bool success, string error, Guid id, string firstName, string lastName, DateTime dateOfBirth, int numberOfArrivals, DateTime checkInDate)
        {
            Success = success;
            Error = error;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            NumberOfArrivals = numberOfArrivals;
            CheckInDate = checkInDate;
        }

        public DailyTrainingGetResult() { }

        public static DailyTrainingGetResult Sucessfull(Guid id, string firstName, string lastName, DateTime dateOfBirth, int numberOfArrivals, DateTime checkInDate) => new(true, string.Empty, id, firstName, lastName, dateOfBirth, numberOfArrivals, checkInDate);
        public static DailyTrainingGetResult Failure(string error) => new(false, error, Guid.Empty, string.Empty, string.Empty, DateTime.Now, 0, DateTime.Now);
    }
}
