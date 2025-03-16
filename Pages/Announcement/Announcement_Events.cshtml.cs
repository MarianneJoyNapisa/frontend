using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ELNET_HomeownersMS.Data; // ✅ Correct namespace for HomeownersContext
using ELNET_HomeownersMS.Models; // ✅ Ensure correct namespace for Announcement model

namespace ELNET_HomeownersMS.Pages.Announcement
{
    public class Announcement_EventsModel : PageModel
    {
        private readonly HomeownersContext _context;

        public Announcement_EventsModel(HomeownersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // ✅ Use Announcement instead of Announcement_EventsModel
        public List<Announcement> TodayAnnouncements { get; private set; } = new();
        public List<Announcement> YesterdayAnnouncements { get; private set; } = new();
        public List<Announcement> WeekAgoAnnouncements { get; private set; } = new();

        public void OnGet()
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var weekAgo = today.AddDays(-7);

            // ✅ Corrected list type
            TodayAnnouncements = _context.Announcements
                .Where(a => a.EventDate.HasValue && a.EventDate.Value.Date == today)
                .ToList();

            YesterdayAnnouncements = _context.Announcements
                .Where(a => a.EventDate.HasValue && a.EventDate.Value.Date == yesterday)
                .ToList();

            WeekAgoAnnouncements = _context.Announcements
                .Where(a => a.EventDate.HasValue && a.EventDate.Value.Date >= weekAgo && a.EventDate.Value.Date < yesterday)
                .ToList();
        }
    }
}
