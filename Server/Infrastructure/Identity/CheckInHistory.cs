using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity
{
    public class CheckInHistory
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid GymUserId { get; set; }
        [Required]
        public DateTime TimeStamp { get; set; }
    }
}
