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
    public class DetailsModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public DetailsModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        public FacilityRequest FacilityRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facilityrequest = await _context.FacilityRequests.FirstOrDefaultAsync(m => m.FacilityRequestId == id);
            if (facilityrequest == null)
            {
                return NotFound();
            }
            else
            {
                FacilityRequest = facilityrequest;
            }
            return Page();
        }
    }
}
