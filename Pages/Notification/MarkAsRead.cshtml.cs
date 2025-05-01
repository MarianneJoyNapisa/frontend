using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using HomeownersMS.Services;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Notification
{
    [Authorize]
    public class MarkAsReadModel(INotificationService notificationService) : PageModel
    {
        private readonly INotificationService _notificationService = notificationService;

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(nameIdentifier))
            {
                throw new InvalidOperationException("User identifier is missing.");
            }
            var userId = int.Parse(nameIdentifier);
            await _notificationService.MarkAsRead(id, userId);
            return new OkResult();
        }
    }
}