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
        Task CreateAnnouncementNotification(Announcement announcement, int createdByUserId);
        Task MarkAsRead(int notificationId, int userId);
        Task<int> GetUnreadCount(int userId);
    }

    public class NotificationService : INotificationService
    {
        private readonly HomeownersContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(HomeownersContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public async Task CreateAnnouncementNotification(Announcement announcement, int createdByUserId)
        {
            // Create the base notification
            var notification = new Notification
            {
                Title = $"New Announcement: {announcement.Title}",
                Message = announcement.Content ?? string.Empty,
                Url = $"/Announcement/Details/{announcement.AnnouncementId}",
                AnnouncementId = announcement.AnnouncementId,
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
                    notification.Message,
                    notification.CreatedAt,
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