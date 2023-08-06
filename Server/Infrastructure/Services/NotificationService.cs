using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.Notifications;
using Application.Enums;

namespace Infrastructure.Services
{
    internal class NotificationService : INotificationService
    {
        private static Dictionary<Guid, string> notifications = new Dictionary<Guid, string>();

        public NotificationResult Add(string message)
        {
            try
            {
                notifications.Add(Guid.NewGuid(), message);

                return NotificationResult.Sucessfull(message);
            }
            catch (Exception exc)
            {
                return NotificationResult.Failure(new Error { Code = ExceptionType.UnableToCreate, Message = "Nije dodati notifikaciju. " + exc.Message });
            }
        }

        public NotificationDeleteAllResult DeleteAll()
        {
            try
            {
                notifications.Clear();

                return NotificationDeleteAllResult.Sucessfull();
            }
            catch (Exception exc)
            {
                return NotificationDeleteAllResult.Failure(new Error { Code = ExceptionType.UnableToDelete, Message = "Nije moguće obrisati notifikacije. " + exc.Message });
            }
        }

        public NotificationDeleteAllResult DeleteOne(Guid id)
        {
            if (!notifications.ContainsKey(id))
            {
                return NotificationDeleteAllResult.Failure(new Error { Code = ExceptionType.UnableToDelete, Message = "Nije moguće obrisati notifikacije. " });
            }

            try
            {
                notifications.Remove(id);

                return NotificationDeleteAllResult.Sucessfull();
            }
            catch (Exception exc)
            {
                return NotificationDeleteAllResult.Failure(new Error { Code = ExceptionType.UnableToDelete, Message = "Nije moguće obrisati notifikacije. " + exc.Message });
            }
        }

        public GetNotificationResult GetAll()
        {
            try
            {
                return GetNotificationResult.Sucessfull(notifications);
            }
            catch (Exception exc)
            {
                return GetNotificationResult.Failure(new Error { Code = ExceptionType.UnableToCreate, Message = "Nije moguće dobiti notifikacije. " + exc.Message });
            }
        }
    }
}