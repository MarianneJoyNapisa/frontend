using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Data;
using HomeownersMS.Models;
using HomeownersMS.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


namespace HomeownersMS.Pages_Admin_Announcements
{
    [Authorize(Roles="admin")]

    public class CreateModel(HomeownersContext context, INotificationService notificationService) : PageModel
    {
        private readonly HomeownersContext _context = context;
        private readonly INotificationService _notificationService = notificationService;

        [BindProperty]
        public Announcement NewAnnouncement { get; set; } = new();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Prepend the Philippine country code (+63) to the phone number
            if (!string.IsNullOrEmpty(NewAnnouncement.ContactNumber))
            {
                NewAnnouncement.ContactNumber = "+63" + NewAnnouncement.ContactNumber.Trim();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                NewAnnouncement.CreatedBy = int.Parse(userId);
                var admin = await _context.Admins.FindAsync(NewAnnouncement.CreatedBy);
                if (admin != null)
                {
                    NewAnnouncement.Admin = admin;
                }
            }

            NewAnnouncement.CreatedAt = DateTime.Now;
            _context.Announcements.Add(NewAnnouncement);
            await _context.SaveChangesAsync();

            // Send notification after announcement is created
            if (NewAnnouncement.AnnouncementId > 0)
            {
                await _notificationService.CreateAnnouncementNotification( 
                    NewAnnouncement,
                    NewAnnouncement.CreatedBy ?? 0
                );
            }

            return RedirectToPage("./Announcements");
        }
    }
}