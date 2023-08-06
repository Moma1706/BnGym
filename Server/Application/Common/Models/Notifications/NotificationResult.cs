using Application.Common.Models.BaseResult;
using Application.Common.Models.GymUser;
using Application.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models.Notifications
{
    public class NotificationResult
    {
        public Guid Id { get; set; }
        public string Notification { get; set; }
        public bool Success { get; set; }
        public Error Error { get; set; }

        public NotificationResult(string notification, bool sucessfull, Error error)
        {
            Notification = notification;
            Success = sucessfull;
            Error = error;
        }

        public NotificationResult()
        { }

        public NotificationResult(Error message)
        {
            Error = message;
        }

        public static NotificationResult Sucessfull(string notification) => new NotificationResult(notification, true, new Error { Code = 0, Message = string.Empty });

        public static NotificationResult Failure(Error error) => new NotificationResult(string.Empty, false, error);
    }
}