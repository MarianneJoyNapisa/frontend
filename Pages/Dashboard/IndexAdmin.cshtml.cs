using HomeownersMS.Models;
using HomeownersMS.Services;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace HomeownersMS.Pages.Dashboard
{
    [Authorize(Roles = "admin")]
    public class IndexAdminModel(HomeownersContext context,
                         INotificationService notificationService,
                         IConfiguration configuration) : PageModel
    {
        private readonly HomeownersContext _context = context;
        private readonly INotificationService _notificationService = notificationService;
        private readonly IConfiguration _configuration = configuration;

        [BindProperty]
        public bool CanSendNotification { get; set; }

        public DateTime? LastNotificationTime { get; set; }
        public TimeSpan? TimeUntilNextNotification { get; set; }

        public async Task OnGetAsync()
        {
            // Check when the last notification was sent
            var lastNotification = await _context.Notifications
                .Where(n => n.MessageType == MessageTypes.announcement && 
                           n.Title.Contains("Popular Community Posts"))
                .OrderByDescending(n => n.CreatedAt)
                .FirstOrDefaultAsync();

            LastNotificationTime = lastNotification?.CreatedAt;
            
            // if (LastNotificationTime.HasValue)
            // {
            //     var cooldown = TimeSpan.FromHours(1);
            //     var nextAllowedTime = LastNotificationTime.Value.Add(cooldown);
            //     TimeUntilNextNotification = nextAllowedTime - DateTime.Now;
            //     CanSendNotification = DateTime.Now >= nextAllowedTime;
            // }
            // else
            // {
            //     CanSendNotification = true;
            // }
            CanSendNotification = true;
        }

        public async Task<IActionResult> OnPostSendPopularPostsNotificationAsync()
        {
            var popularPosts = await _context.CommunityPosts
                .Include(p => p.User)
                .OrderByDescending(p => p.Vote)
                .Take(3)
                .ToListAsync();

            if (!popularPosts.Any())
            {
                TempData["ErrorMessage"] = "No community posts found to notify about.";
                return RedirectToPage();
            }

            var nameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(nameIdentifier))
            {
                throw new InvalidOperationException("NameIdentifier claim is missing.");
            }
            var adminUserId = int.Parse(nameIdentifier);
            var title = "Popular Community Posts This Week";

            // Start building the HTML-formatted message
            var message = new StringBuilder();
            message.Append("<div class='notification-content'>");
            message.Append("<h5 class='mb-3'>Here are the most popular discussions:</h5>");
            message.Append("<hr>");
            message.Append("<ul class='list-unstyled'>");

            foreach (var post in popularPosts)
            {
                // Store the author's privilege level
                var authorRole = post.User?.Privilege?.ToString().ToLower() ?? "unknown";
                var roleBadgeClass = authorRole switch
                {
                    "admin" => "bg-danger",
                    "staff" => "bg-primary",
                    "resident" => "bg-success",
                    _ => "bg-secondary"
                };

                message.Append("<li class='mb-3'>");
                message.Append($"<div class='d-flex justify-content-between align-items-start'>");
                message.Append($"<span class='fw-bold'>{post.Title}</span>");
                message.Append($"<span class='badge text-light {roleBadgeClass} ms-2'>{authorRole}</span>");
                message.Append("</div>");
                
                message.Append($"<div class='text-muted mt-1 mb-2'>{post.Content}</div>");
                
                // Add voting information if available
                message.Append($"<div class='d-flex align-items-center small'>");
                message.Append($"<span class='me-2'><i class='bi bi-arrow-up-circle-fill text-success'></i> {post.Vote}</span>");
                message.Append($"<span><i class='bi bi-chat-left-text me-1'></i> {post.Comments?.Count ?? 0} comments</span>");
                message.Append("</div>");
                
                // Add the "Join conversation" button with post-specific URL
                message.Append($"<div class='mt-2'>");
                message.Append($"<a href='/Community/Community/#community-post-id-{post.CommunityPostId}' class='btn btn-sm btn-outline-primary'>Join this conversation</a>");
                message.Append("</div>");
                message.Append("<hr>");
                
                message.Append("</li>");
            }

            message.Append("</ul>");
            message.Append("</div>");

            // Final message with HTML formatting
            var formattedMessage = message.ToString();

            // Send to all users (they'll see a filtered version based on their role)
            await _notificationService.CreateNotification(
                title,
                formattedMessage,
                "/Notification/Notifications",
                MessageTypes.announcement,
                adminUserId);

            TempData["SuccessMessage"] = "Popular posts notification sent!";
            return RedirectToPage();
        }
    }
}