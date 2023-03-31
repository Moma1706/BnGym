using Application.Common.Models.Auth;
using Application.Common.Models.GymWorker;
using Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public bool IsStudent { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsFrozen { get; set; }
        public DateTime FreezeDate { get; set; }
        public bool IsInactive { get; set; }
        public DateTime LastCheckIn { get; set; }
        public GymUserType Type { get; set; }
        public int NumberOfArrivals { get; set; }
        
        public bool Success { get; set; }
        public string Error { get; set; }

        public GymUserGetResult(bool successful, string error, Guid id, int userId, string firstName, string lastName, string email, bool isStudent, DateTime expiresOn, bool isBlocked, bool isFrozen, DateTime freezeDate, bool isInactive, DateTime lastCheckIn, GymUserType type, int numberOfArrivals)
        {
            Success = successful;
            Error = error;
            Id = id;
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsStudent = isStudent;
            ExpiresOn = expiresOn;
            IsBlocked = isBlocked;
            IsFrozen = isFrozen;
            FreezeDate = freezeDate;
            IsInactive = isInactive;
            LastCheckIn = lastCheckIn;
            Type = type;
            NumberOfArrivals = numberOfArrivals;
        }

        public GymUserGetResult() {}


        public static GymUserGetResult Sucessfull(Guid id, int userId, string firstName, string lastName, string email, bool isStudent, DateTime expiresOn, bool isBlocked, bool isFrozen, DateTime freezeDate, bool isInactive, DateTime lastCheckIn, GymUserType type, int numberOfArrivals) => new(true, string.Empty, id, userId, firstName, lastName, email, isStudent, expiresOn, isBlocked, isFrozen, freezeDate, isInactive, lastCheckIn, type, numberOfArrivals);
    }
}
