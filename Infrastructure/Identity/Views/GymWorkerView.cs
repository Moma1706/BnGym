using System;
namespace Infrastructure.Identity.Views
{
    public class GymWorkerView
	{
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsBlocked { get; set; }
        public int RoleId { get; set; }
    }
}

