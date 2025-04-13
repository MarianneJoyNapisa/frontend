using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Data;
using HomeownersMS.Models;
using System.Security.Claims;

namespace HomeownersMS.Pages_Admin_Announcements
{
    public class CreateModel : PageModel
    {
        private readonly HomeownersContext _context;

        public CreateModel(HomeownersContext context)
        {
            _context = context;
        }

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

            return RedirectToPage("./Announcements");
        }
    }
}