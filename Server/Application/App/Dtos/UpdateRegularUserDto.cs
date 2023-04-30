using System;
using Application.Enums;

namespace Application.App.Dtos
{
    public class UpdateRegularUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
