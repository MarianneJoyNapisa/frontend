using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages_Admin_Announcements
{
    [Authorize(Roles="admin")]

    public class EditModel : PageModel
    {
        private readonly HomeownersContext _context;

        public EditModel(HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Announcement Announcement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            Announcement = announcement;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync(
                announcement,
                "Announcement",
                a => a.Title, a => a.Content, a => a.EventDate, a => a.EventTime,
                a => a.BlocksAffected, a => a.Office, a => a.ContactNumber))
            {
                await _context.SaveChangesAsync();
                return RedirectToPage("./Announcements");
            }

            return Page();
        }
    }
}