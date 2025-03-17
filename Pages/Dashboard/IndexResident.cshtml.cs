using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeownersMS.Pages.Dashboard
{
    [Authorize(Roles = "admin,resident")]
    public class IndexResidentModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        // Ensure only one constructor is defined
        public IndexResidentModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<Models.Announcement> LatestAnnouncements { get; private set; } = new();
        public List<CalendarEvent> CalendarEvents { get; private set; } = new();

        public void OnGet()
        {
            // Fetch the 3 latest announcements ordered by CreatedAt (newest first)
            LatestAnnouncements = _context.Announcements
                .OrderByDescending(a => a.CreatedAt)
                .Take(3)
                .ToList();

            // Format announcements as calendar events
            CalendarEvents = _context.Announcements
                .Select(a => new CalendarEvent
                {
                    Title = a.Title,
                    Start = $"{a.EventDate:yyyy-MM-dd}", // Use string interpolation to format DateOnly
                    Description = a.Content,
                    EventTime = $"{a.EventTime:hh\\:mm tt}" // Format TimeOnly as string
                })
                .ToList();
        }

        public class CalendarEvent
        {
            public string Title { get; set; }
            public string Start { get; set; } // Start is a string in "yyyy-MM-dd" format
            public string Description { get; set; }
            public string EventTime { get; set; } // EventTime is a string in "hh:mm tt" format
        }
    }
}