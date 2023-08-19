using Application.Common.Interfaces;
using Application.Common.Models.BaseResult;
using Application.Common.Models.Notifications;
using Application.Enums;
using Google.Apis.Gmail.v1.Data;
using Infrastructure.Data;
using Infrastructure.Hubs;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    internal class NotificationService : INotificationService
    {
        private static List<KeyValuePair<Guid, string>> notifications = new List<KeyValuePair<Guid, string>>();
        //private static Dictionary<Guid, string> notifications = new Dictionary<Guid, string>();

        private readonly IHubContext<NotificationHub> _hub;

        public NotificationService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task<NotificationResult> Add(string message)
        {
            try
            {
                notifications.Add(new KeyValuePair<Guid, string>(Guid.NewGuid(), message));
                // emit messageSent
                await _hub.Clients.All.SendAsync("messageSent", message);

                return NotificationResult.Sucessfull(message);
            }
            catch (Exception exc)
            {
                return NotificationResult.Failure(new Error { Code = ExceptionType.UnableToCreate, Message = "Nije dodati notifikaciju. " + exc.Message });
            }
        }

        public async Task<NotificationDeleteAllResult> DeleteAll()
        {
            try
            {
                notifications.Clear();
                // emit noNotifications
                await _hub.Clients.All.SendAsync("noNotifications", "Nema notifikacija");

                return NotificationDeleteAllResult.Sucessfull();
            }
            catch (Exception exc)
            {
                return NotificationDeleteAllResult.Failure(new Error { Code = ExceptionType.UnableToDelete, Message = "Nije moguće obrisati notifikacije. " + exc.Message });
            }
        }

        public async Task<NotificationDeleteAllResult> DeleteOne(Guid id)
        {
            bool containsKey = notifications.Any(item => item.Key == id);
            if (!containsKey)
            {
                return NotificationDeleteAllResult.Failure(new Error { Code = ExceptionType.UnableToDelete, Message = "Nije moguće obrisati notifikacije. " });
            }

            try
            {
                notifications.RemoveAll(item => item.Key == id);
                // ako je dictionary prazan -> emit noNotifications
                if (notifications.Count() == 0)
                    await _hub.Clients.All.SendAsync("noNotifications", "Nema notifikacija");

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