using System;
using Application.Enums;

namespace Application.GymUser.Dtos
{
	public class UpdateGymUserDto
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public GymUserType Type { get; set; }
	}
}
