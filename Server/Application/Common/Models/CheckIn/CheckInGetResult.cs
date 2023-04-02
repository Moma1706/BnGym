using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.CheckIn
{
    public class CheckInGetResult
    {
        public Guid Id { get; set; }
        public Guid GymUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime TimeStamp { get; set; }

        public CheckInGetResult()
        {
            
        }

    }
}
