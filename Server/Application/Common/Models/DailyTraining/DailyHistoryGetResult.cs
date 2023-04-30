using System;
using Application.Common.Models.BaseResult;

namespace Application.Common.Models.DailyTraining
{
	public class DailyHistoryGetResult
	{
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CheckInDate { get; set; }

        public bool Success { get; set; }
        public Error Error { get; set; }

        public DailyHistoryGetResult(bool success, Error error, Guid id, string firstName, string lastName, DateTime dateOfBirth, DateTime checkInDate)
        {
            Success = success;
            Error = error;
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            CheckInDate = checkInDate;
        }

        public DailyHistoryGetResult() { }

        public static DailyHistoryGetResult Sucessfull(Guid id, string firstName, string lastName, DateTime dateOfBirth, DateTime checkInDate) => new(true, new Error { Code = 0, Message = string.Empty}, id, firstName, lastName, dateOfBirth, checkInDate);
        public static DailyHistoryGetResult Failure(Error error) => new(false, error, Guid.Empty, string.Empty, string.Empty, DateTime.UtcNow, DateTime.UtcNow);

    }
}

