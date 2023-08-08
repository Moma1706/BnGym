using Application.Common.Models.BaseResult;

namespace Application.Common.Models.Notifications
{
    public class GetNotificationResult
    {
        public List<KeyValuePair<Guid, string>> Notifications { get; set; }

        public bool Sucess { get; set; }
        public Error Error { get; set; }

        public GetNotificationResult(List<KeyValuePair<Guid, string>> notifications, bool sucess, Error error)
        {
            Notifications = notifications;
            Sucess = sucess;
            Error = error;
        }

        public static GetNotificationResult Sucessfull(List<KeyValuePair<Guid, string>> notifications) => new GetNotificationResult(notifications, true, new Error { Code = 0, Message = string.Empty });

        public static GetNotificationResult Failure(Error error) => new GetNotificationResult(new List<KeyValuePair<Guid, string>>(), false, error);
    }
}