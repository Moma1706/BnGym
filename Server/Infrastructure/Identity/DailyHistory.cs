using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Identity
{
    public class DailyHistory
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid DailyUserId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }
    }
}
