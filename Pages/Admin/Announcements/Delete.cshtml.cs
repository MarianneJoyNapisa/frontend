using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;

namespace HomeownersMS.Pages_Admin_Announcements
{
    public class DeleteModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public DeleteModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Announcement Announcement { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FirstOrDefaultAsync(m => m.AnnouncementId == id);

            if (announcement == null)
            {
                return NotFound();
            }
            else
            {
                Announcement = announcement;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                Announcement = announcement;
                _context.Announcements.Remove(Announcement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
