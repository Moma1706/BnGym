using Application.Common.Models.BaseResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Notifications
{
    public class NotificationDeleteAllResult
    {
        public string message { get; set; }
        public bool Success { get; set; }
        public Error Error { get; set; }

        public NotificationDeleteAllResult(bool sucessfull, Error error)
        {
            Success = sucessfull;
            Error = error;
            message = "Sve Notifikacije obrisane!";
        }

        public NotificationDeleteAllResult()
        { }

        public NotificationDeleteAllResult(Error message)
        {
            Error = message;
        }

        public static NotificationDeleteAllResult Sucessfull() => new NotificationDeleteAllResult(true, new Error { Code = 0, Message = string.Empty });

        public static NotificationDeleteAllResult Failure(Error error) => new NotificationDeleteAllResult(false, error);
    }
}