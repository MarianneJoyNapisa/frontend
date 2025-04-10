using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;
using System.Security.Claims;

namespace HomeownersMS.Pages_Admin_Announcements
{
    public class IndexModel : PageModel
    {
        private readonly HomeownersContext _context;

        public IndexModel(HomeownersContext context)
        {
            _context = context;
        }

        public IList<Announcement> Announcement { get; set; } = default!;

        [BindProperty]
        public Announcement NewAnnouncement { get; set; } = new();

        public async Task OnGetAsync()
        {
            Announcement = await _context.Announcements
                .Include(a => a.Admin)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
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

            return RedirectToPage();
        }

        

        public async Task<IActionResult> OnPostEditAsync(int AnnouncementId)
        {
            var announcement = await _context.Announcements.FindAsync(AnnouncementId);
            if (announcement == null)
            {
                return NotFound();
            }

            // Manually bind each property
            announcement.Title = Request.Form["Title"];
            announcement.Content = Request.Form["Content"];
            announcement.EventDate = DateOnly.Parse(Request.Form["EventDate"]);
            announcement.EventTime = TimeOnly.Parse(Request.Form["EventTime"]);
            announcement.BlocksAffected = Request.Form["BlocksAffected"];
            announcement.Office = Request.Form["Office"];
            if (int.TryParse(Request.Form["ContactNumber"], out int contactNumber))
            {
                announcement.ContactNumber = contactNumber;
            }
            else
            {
                announcement.ContactNumber = null; // Handle invalid input gracefully
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnnouncementExists(AnnouncementId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement != null)
            {
                _context.Announcements.Remove(announcement);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetDetailsPartialAsync(int id)
        {
            var announcement = await _context.Announcements
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(a => a.AnnouncementId == id);

            if (announcement == null)
            {
                return NotFound();
            }

            return Partial("_DetailsPartial", announcement);
        }

        public async Task<IActionResult> OnGetEditPartialAsync(int id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return NotFound();
            }

            return Partial("_EditPartial", announcement);
        }

        private bool AnnouncementExists(int id)
        {
            return _context.Announcements.Any(a => a.AnnouncementId == id);
        }
    }
}