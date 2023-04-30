using System;
namespace Infrastructure.Identity.Views
{
    public class DailyTrainingView
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime LastCheckIn { get; set; }
        public int numberOfArrivalsCurrentMonth { get; set; }
        public int NumberOfArrivalsLastMonth { get; set; }
    }
}
