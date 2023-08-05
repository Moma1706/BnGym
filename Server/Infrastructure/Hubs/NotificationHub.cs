using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task Send(string message)
        {
            await Clients.All.SendAsync(WebSocketActions.MESSAGE_RECEIVED, message);
        }
    }

    public struct WebSocketActions
    {
        public static readonly string MESSAGE_RECEIVED = "messageReceived";
    }
}