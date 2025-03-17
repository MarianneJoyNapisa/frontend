using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HomeownersMS.Pages.Announcement_Events
{
    public class Announcement_EventsModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public Announcement_EventsModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        // ✅ Use Announcement instead of Announcement_EventsModel
        public List<Models.Announcement> TodayAnnouncements { get; private set; } = new();
        public List<Models.Announcement> YesterdayAnnouncements { get; private set; } = new();
        public List<Models.Announcement> WeekAgoAnnouncements { get; private set; } = new();

        public void OnGet()
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);
            var yesterday = today.AddDays(-1);
            var weekAgo = today.AddDays(-7);

            // ✅ Corrected list type
            TodayAnnouncements = _context.Announcements
                .Where(a => a.EventDate.HasValue && a.EventDate.Value == today)
                .ToList();

            YesterdayAnnouncements = _context.Announcements
                .Where(a => a.EventDate.HasValue && a.EventDate.Value == yesterday)
                .ToList();

            WeekAgoAnnouncements = _context.Announcements
                .Where(a => a.EventDate.HasValue && a.EventDate.Value >= weekAgo && a.EventDate.Value < yesterday)
                .ToList();
        }
    }
}
