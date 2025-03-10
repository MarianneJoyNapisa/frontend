using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;

namespace HomeownersMS.Pages_Admin_FacilityRequests
{
    public class EditModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public EditModel(HomeownersMS.Data.HomeownersContext context)
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

            var facilityrequest =  await _context.FacilityRequests.FirstOrDefaultAsync(m => m.FacilityRequestId == id);
            if (facilityrequest == null)
            {
                return NotFound();
            }
            FacilityRequest = facilityrequest;
           ViewData["FacilityId"] = new SelectList(_context.Facilities, "FacilityId", "FacilityId");
           ViewData["ResidentId"] = new SelectList(_context.Residents, "UserId", "UserId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(FacilityRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacilityRequestExists(FacilityRequest.FacilityRequestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool FacilityRequestExists(int id)
        {
            return _context.FacilityRequests.Any(e => e.FacilityRequestId == id);
        }
    }
}
