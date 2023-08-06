using Application.Common.Models.Notifications;

namespace Application.Common.Interfaces
{
    public interface INotificationService
    {
        GetNotificationResult GetAll();

        NotificationResult Add(string message);

        NotificationDeleteAllResult DeleteOne(Guid id);

        NotificationDeleteAllResult DeleteAll();
    }
}