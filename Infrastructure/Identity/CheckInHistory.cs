using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class CheckInHistory
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
