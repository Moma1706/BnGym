using System;
using Application.Enums;

namespace Infrastructure.Identity.Views
{
	public class GymUserView
	{
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsFrozen { get; set; }
        public DateTime FreezeDate { get; set; }
        public bool IsInactive { get; set; }
        public DateTime LastCheckIn { get; set; }
        public GymUserType Type { get; set; }
        public int NumberOfArrivals { get; set; }
        public string Address { get; set; }
    }
}

