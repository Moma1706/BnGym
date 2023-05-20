using System;
using Application.Enums;

namespace Application.DailyUser.Dtos
{
    public class UpdateDailyUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
