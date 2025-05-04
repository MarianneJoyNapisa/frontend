// Services/NotificationService.cs
using HomeownersMS.Data;
using HomeownersMS.Hubs;
using HomeownersMS.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HomeownersMS.Services
{
    public interface INotificationService
    {
        Task CreateNotification(string title, string message, string url, MessageTypes messageType, int createdByUserId);

        Task CreateNotificationForGroup(string title, string message, string url, MessageTypes messageType, int createdByUserId, List<int> recipientUserIds);
        Task MarkAsRead(int notificationId, int userId);
        Task<int> GetUnreadCount(int userId);
    }

    public class NotificationService(HomeownersContext context, IHubContext<NotificationHub> hubContext) : INotificationService
    {
        private readonly HomeownersContext _context = context;
        private readonly IHubContext<NotificationHub> _hubContext = hubContext;

        public async Task CreateNotification(string title, string message, string? url, MessageTypes messageType, int createdByUserId)
        {
            // Create the base notification
            var notification = new Notification
            {
                Title = title,
                Message = message ?? string.Empty,
                MessageType = messageType,
                Url = url,
                CreatedByUserId = createdByUserId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Get all users (residents and staff)
            var users = await _context.Users
                .Where(u => u.Privilege == Privileges.resident || u.Privilege == Privileges.staff)
                .ToListAsync();

            // Create user notifications
            var userNotifications = new List<UserNotification>();
            
            foreach (var user in users)
            {
                var userNotification = new UserNotification
                {
                    UserId = user.UserId,
                    NotificationId = notification.NotificationId,
                    IsRead = false,
                    User = user,  // Set the required navigation property
                    Notification = notification  // Set the required navigation property
                };
                userNotifications.Add(userNotification);
            }

            _context.UserNotifications.AddRange(userNotifications);
            await _context.SaveChangesAsync();

            // Notify all users in real-time
            foreach (var user in users)
            {
                await _hubContext.Clients.Group(user.UserId.ToString()).SendAsync("ReceiveNotification", new
                {
                    notification.NotificationId,
                    notification.Title,
                    MessageType = notification.MessageType.ToString(),
                    notification.Message,
                    notification.CreatedAt,
                    notification.Url,
                    IsRead = false
                });
            }
        }

        public async Task CreateNotificationForGroup(string title, string message, string? url, MessageTypes messageType, int createdByUserId, List<int> recipientUserIds)
        {
            if (recipientUserIds == null || !recipientUserIds.Any())
            {
                throw new ArgumentException("At least one recipient user ID must be specified");
            }

            // Create the base notification
            var notification = new Notification
            {
                Title = title,
                Message = message ?? string.Empty,
                MessageType = messageType,
                Url = url,
                CreatedByUserId = createdByUserId
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            // Get only the specified users
            var users = await _context.Users
                .Where(u => recipientUserIds.Contains(u.UserId))
                .ToListAsync();

             // Create user notifications
            var userNotifications = new List<UserNotification>();
            
            foreach (var user in users)
            {
                var userNotification = new UserNotification
                {
                    UserId = user.UserId,
                    NotificationId = notification.NotificationId,
                    IsRead = false,
                    User = user,  // Set the required navigation property
                    Notification = notification  // Set the required navigation property
                };
                userNotifications.Add(userNotification);
            }

            _context.UserNotifications.AddRange(userNotifications);
            await _context.SaveChangesAsync();

            // Notify all users in real-time
            foreach (var user in users)
            {
                await _hubContext.Clients.Group(user.UserId.ToString()).SendAsync("ReceiveNotification", new
                {
                    notification.NotificationId,
                    notification.Title,
                    MessageType = notification.MessageType.ToString(),
                    notification.Message,
                    notification.CreatedAt,
                    notification.Url,
                    IsRead = false
                });
            }
        }

        public async Task MarkAsRead(int notificationId, int userId)
        {
            var userNotification = await _context.UserNotifications
                .Include(un => un.User)
                .Include(un => un.Notification)
                .FirstOrDefaultAsync(un => un.NotificationId == notificationId && un.UserId == userId);

            if (userNotification != null && !userNotification.IsRead)
            {
                userNotification.IsRead = true;
                userNotification.ReadAt = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetUnreadCount(int userId)
        {
            return await _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .CountAsync();
        }
    }
}