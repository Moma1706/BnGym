using System;
namespace Infrastructure.Identity.Views
{
	public class DailyHistoryView
	{
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CheckInDate { get; set; }
        public int NumberOfArrivalsCurrentMonth { get; set; }
        public int NumberOfArrivalsLastMonth { get; set; }
    }
}
