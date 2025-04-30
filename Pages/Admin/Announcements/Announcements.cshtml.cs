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
        public IList<Announcement> TodayAnnouncements { get; set; } = new List<Announcement>();
        public IList<Announcement> YesterdayAnnouncements { get; set; } = new List<Announcement>();
        public IList<Announcement> WeekAnnouncements { get; set; } = new List<Announcement>();
        public IList<Announcement> MonthAnnouncements { get; set; } = new List<Announcement>();

        [BindProperty]
        public Announcement NewAnnouncement { get; set; } = new();

        public async Task OnGetAsync()
        {
            DateTime today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var weekStart = today.AddDays(-7);
            var monthStart = today.AddMonths(-1);

            var announcements = await _context.Announcements
                .Include(a => a.Admin)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            TodayAnnouncements = announcements
                .Where(a => a.CreatedAt.Date == today.Date)
                .ToList();

            YesterdayAnnouncements = announcements
                .Where(a => a.CreatedAt.Date == yesterday.Date)
                .ToList();

            WeekAnnouncements = announcements
                .Where(a => a.CreatedAt.Date >= weekStart.Date && a.CreatedAt.Date < yesterday.Date)
                .ToList();

            MonthAnnouncements = announcements
                .Where(a => a.CreatedAt.Date < weekStart.Date)
                .ToList();
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
    }
}