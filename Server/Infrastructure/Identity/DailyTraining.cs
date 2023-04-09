using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity
{
    public class DailyTraining
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [DefaultValue(0)]
        public int NumberOfArrivals { get; set; }
    }
}
