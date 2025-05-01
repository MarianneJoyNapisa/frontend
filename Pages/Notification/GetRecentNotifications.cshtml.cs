using HomeownersMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Notification
{
    [Authorize]
    public class GetRecentNotificationsModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User identifier claim is missing.");
            }
            var userId = int.Parse(userIdClaim);
            
            var notifications = await _context.UserNotifications
                .Include(un => un.Notification)
                .Where(un => un.UserId == userId)
                .OrderByDescending(un => un.Notification.CreatedAt)
                .Take(5)
                .Select(un => new
                {
                    un.NotificationId,
                    un.Notification.Title,
                    un.Notification.Url,
                    un.Notification.Message,
                    un.Notification.CreatedAt,
                    un.IsRead
                })
                .ToListAsync();

            return new JsonResult(notifications);
        }
    }
}