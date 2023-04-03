using System;
using Application.Enums;

namespace Infrastructure.Identity.Views
{
    public class CheckInHistoryView
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid GymUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime LastCheckIn { get; set; }
        public int NumberOfArrivals { get; set; }
    }
}

