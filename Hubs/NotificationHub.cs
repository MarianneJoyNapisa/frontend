// Hubs/NotificationHub.cs
using Microsoft.AspNetCore.SignalR;

namespace HomeownersMS.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task JoinNotificationGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        
        public async Task LeaveNotificationGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
    }
}