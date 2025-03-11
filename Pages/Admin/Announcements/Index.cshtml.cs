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
    public class IndexModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public IndexModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        public IList<Announcement> Announcement { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Announcement = await _context.Announcements
                .Include(a => a.Admin).ToListAsync();
        }
    }
}
