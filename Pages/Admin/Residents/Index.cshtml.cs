using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages_Admin_Residents
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public IndexModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        public IList<Resident> Resident { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Resident = await _context.Residents
                .Include(r => r.User).ToListAsync();
        }
    }
}
