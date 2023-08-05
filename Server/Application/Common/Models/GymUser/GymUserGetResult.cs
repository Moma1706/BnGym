using Application.Common.Models.Auth;
using Application.Common.Models.BaseResult;
using Application.Common.Models.GymWorker;
using Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.GymUser
{
    public class GymUserGetResult
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsFrozen { get; set; }
        public string FreezeDate { get; set; }
        public bool IsInActive { get; set; }
        public string LastCheckIn { get; set; }
        public GymUserType Type { get; set; }
        public string Address { get; set; }
        public int NumberOfArrivalsLastMonth { get; set; }
        public int NumberOfArrivalsCurrentMonth { get; set; }

        public bool Success { get; set; }
        public Error Error { get; set; }

        public GymUserGetResult(bool successful, Error error, Guid id, int userId, string firstName, string lastName, string email, DateTime expiresOn, bool isBlocked, bool isFrozen, string freezeDate, bool isInActive, string lastCheckIn, GymUserType type, string address, int numberOfArrivalsLastMonth, int numberOfArrivalsCurrentMonth)
        {
            Success = successful;
            Error = error;
            Id = id;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ExpiresOn = expiresOn;
            IsBlocked = isBlocked;
            IsFrozen = isFrozen;
            FreezeDate = freezeDate;
            IsInActive = isInActive;
            LastCheckIn = lastCheckIn;
            Type = type;
            Address = address;
            NumberOfArrivalsCurrentMonth = numberOfArrivalsCurrentMonth;
            NumberOfArrivalsLastMonth = numberOfArrivalsLastMonth;
        }

        public GymUserGetResult()
        { }

        public GymUserGetResult(Error error)
        {
            Error = error;
        }

        public static GymUserGetResult Sucessfull(Guid id, int userId, string firstName, string lastName, string email, DateTime expiresOn, bool isBlocked, bool isFrozen, string freezeDate, bool isInActive, string lastCheckIn, GymUserType type, string address, int numberOfArrivalsLastMonth, int numberOfArrivalsCurrentMonth) => new(true, new Error { Code = 0, Message = string.Empty }, id, userId, firstName, lastName, email, expiresOn, isBlocked, isFrozen, freezeDate, isInActive, lastCheckIn, type, address, numberOfArrivalsLastMonth, numberOfArrivalsCurrentMonth);

        public static GymUserGetResult Failure(Error error) => new(false, error, Guid.Empty, 0, string.Empty, string.Empty, string.Empty, DateTime.MinValue, false, false, string.Empty, false, string.Empty, 0, string.Empty, 0, 0);
    }
}