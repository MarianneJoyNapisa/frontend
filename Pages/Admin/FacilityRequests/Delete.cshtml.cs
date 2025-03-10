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
    public class DeleteModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public DeleteModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var facilityrequest = await _context.FacilityRequests.FindAsync(id);
            if (facilityrequest != null)
            {
                FacilityRequest = facilityrequest;
                _context.FacilityRequests.Remove(FacilityRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
