using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HomeownersMS.Data;
using HomeownersMS.Models;

namespace HomeownersMS.Pages.Announcement
{
    [Authorize(Roles="resident,admin")]
    public class DetailsModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public Models.Announcement? Announcements { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Announcements = await _context.Announcements
                .FirstOrDefaultAsync(a => a.AnnouncementId == id);
            
            if (Announcements == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}