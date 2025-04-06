using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data;

namespace HomeownersMS.Pages.Reservation
{
    public class ReserveModel : PageModel
    {
        private readonly HomeownersContext _context;

        public ReserveModel(HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]  // Allows binding from query string
        public int FacilityId { get; set; }

        public Facility Facility { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Facility = await _context.Facilities
                .FirstOrDefaultAsync(f => f.FacilityId == FacilityId) ?? throw new InvalidOperationException("Facility not found.");

            if (Facility == null)
            {
                return NotFound();  // 404 if facility doesn't exist
            }

            return Page();
        }
    }
}