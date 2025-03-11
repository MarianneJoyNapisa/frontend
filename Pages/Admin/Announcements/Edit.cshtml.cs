using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;

namespace HomeownersMS.Pages_Admin_Announcements
{
    public class EditModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public EditModel(HomeownersMS.Data.HomeownersContext context)
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

            var announcement =  await _context.Announcements.FirstOrDefaultAsync(m => m.AnnouncementId == id);
            if (announcement == null)
            {
                return NotFound();
            }
            Announcement = announcement;
           ViewData["CreatedBy"] = new SelectList(_context.Admins, "UserId", "UserId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Announcement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(Announcement.AnnouncementId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(e => e.AnnouncementId == id);
        }
    }
}
