using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Announcement_Events
{
    [Authorize(Roles="admin,resident")]

    public class Announcement_EventsModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public Announcement_EventsModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // Properties to hold filtered announcements
        public List<Models.Announcement> TodayAnnouncements { get; private set; } = new();
        public List<Models.Announcement> YesterdayAnnouncements { get; private set; } = new();
        public List<Models.Announcement> WeekAgoAnnouncements { get; private set; } = new();

        // Query parameters for filtering
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedDate { get; set; }

        public void OnGet()
        {
            DateTime today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var weekAgo = today.AddDays(-7);

            // Base query for announcements
            var announcementsQuery = _context.Announcements.AsQueryable();

            // Apply search term filter
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                // Use EF.Functions.Like for case-insensitive search
                var searchTermPattern = $"%{SearchTerm}%";
                announcementsQuery = announcementsQuery.Where(a =>
                    EF.Functions.Like(a.Title, searchTermPattern) ||
                    EF.Functions.Like(a.Content, searchTermPattern)
                );
            }

            // Apply date filter
            if (!string.IsNullOrEmpty(SelectedDate) && DateTime.TryParse(SelectedDate, out var filterDate))
            {
                announcementsQuery = announcementsQuery.Where(a => a.CreatedAt.Date == filterDate.Date);
            }

            // Fetch and categorize announcements
            TodayAnnouncements = announcementsQuery
                .Where(a => a.CreatedAt.Date == today.Date)
                .ToList();

            YesterdayAnnouncements = announcementsQuery
                .Where(a => a.CreatedAt.Date == yesterday.Date)
                .ToList();

            WeekAgoAnnouncements = announcementsQuery
                .Where(a => a.CreatedAt.Date < yesterday.Date)
                .ToList();
        }
    }
}