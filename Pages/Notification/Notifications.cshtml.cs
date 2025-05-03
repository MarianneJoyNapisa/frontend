using HomeownersMS.Models;
using HomeownersMS.Services;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Notifications
{
    [Authorize]
    public class IndexModel(INotificationService notificationService, HomeownersContext context) : PageModel
    {
        private readonly INotificationService _notificationService = notificationService;
        private readonly HomeownersContext _context = context;

        public List<UserNotification> Notifications { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User ID claim is missing.");
            }
            var userId = int.Parse(userIdClaim);
            
            Notifications = await _context.UserNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId)
                .OrderByDescending(un => un.Notification.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostMarkAsReadAsync(int id)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User ID claim is missing.");
            }
            var userId = int.Parse(userIdClaim);
            await _notificationService.MarkAsRead(id, userId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostMarkAllAsReadAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User ID claim is missing.");
            }
            var userId = int.Parse(userIdClaim);
            
            var unreadNotifications = await _context.UserNotifications
                .Where(un => un.UserId == userId && !un.IsRead)
                .ToListAsync();

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}