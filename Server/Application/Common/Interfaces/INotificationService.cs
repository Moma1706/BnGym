using Application.Common.Models.Notifications;

namespace Application.Common.Interfaces
{
    public interface INotificationService
    {
        GetNotificationResult GetAll();

        Task<NotificationResult> Add(string message);

        Task<NotificationDeleteAllResult> DeleteOne(Guid id);

        Task<NotificationDeleteAllResult> DeleteAll();
    }
}