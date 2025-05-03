using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using HomeownersMS.Services;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Notification
{
    [Authorize]
    public class GetUnreadCountModel : PageModel
    {
        private readonly INotificationService _notificationService;

        public GetUnreadCountModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return BadRequest("User identifier is missing.");
            }
            var userId = int.Parse(userIdClaim);
            var count = await _notificationService.GetUnreadCount(userId);
            return new JsonResult(count);
        }
    }
}