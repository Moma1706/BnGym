using System;
using Application.Enums;

namespace Application.DailyTraining.Dtos
{
    public class UpdateDailyTrainingDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
