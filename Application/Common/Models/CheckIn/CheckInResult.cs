using Application.Common.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.CheckIn
{
    public class CheckInResult
    {
        public bool Success { get; set; }
        public Guid Id { get; set; }
        public int UsereId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Error { get; set; }

        public CheckInResult(bool sucess, Guid id, int userId, DateTime timeStamp, string error) 
        {
            Success = sucess;
            Id = id;
            UsereId = userId;
            TimeStamp = timeStamp;
            Error = error;
        }

        public static CheckInResult Sucessfull() => new(true, Guid.Empty, 0, DateTime.UtcNow, string.Empty);
        public static CheckInResult Sucessfull(Guid id, int userId, DateTime timeStamp) => new(true, id, userId, timeStamp, string.Empty);
        public static CheckInResult Failure(string error) => new(false, Guid.Empty, 0, DateTime.UtcNow, error);
    }
}
