using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;

namespace HomeownersMS.Pages_Admin_FacilityRequests
{
    public class IndexModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public IndexModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        public IList<FacilityRequest> FacilityRequest { get;set; } = default!;

        public async Task OnGetAsync()
        {
            FacilityRequest = await _context.FacilityRequests
                .Include(f => f.Facility)
                .Include(f => f.Resident).ToListAsync();
        }
    }
}
