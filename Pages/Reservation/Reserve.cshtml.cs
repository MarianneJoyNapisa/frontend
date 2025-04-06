using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data;

public class ReserveModel : PageModel
{
    private readonly HomeownersContext _context;

    public ReserveModel(HomeownersContext context)
    {
        _context = context;
    }

    [BindProperty]
    public FacilityRequest FacilityRequest { get; set; }

    public Facility Facility { get; set; }

    public async Task<IActionResult> OnGetAsync(int facilityId)
    {
        Facility = await _context.Facilities.FindAsync(facilityId);
        if (Facility == null)
        {
            return NotFound();
        }

        // Pre-fill resident info if logged in
        var resident = await _context.Residents.FirstOrDefaultAsync(r => r.UserId == User.GetUserId());
        if (resident != null)
        {
            FacilityRequest = new FacilityRequest
            {
                FacilityId = facilityId,
                ResidentId = resident.ResidentId,
                FullName = resident.FullName,
                EmailAddress = resident.EmailAddress,
                PhoneNumber = resident.PhoneNumber
            };
        }
        else
        {
            FacilityRequest = new FacilityRequest { FacilityId = facilityId };
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Facility = await _context.Facilities.FindAsync(FacilityRequest.FacilityId);
            return Page();
        }

        _context.FacilityRequests.Add(FacilityRequest);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Reservation/MyReservations");
    }
}
