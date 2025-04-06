using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data;

public class MyReservationsModel : PageModel
{
    private readonly HomeownersContext _context;

    public MyReservationsModel(HomeownersContext context)
    {
        _context = context;
    }

    public List<FacilityRequest> Reservations { get; set; }

    public async Task OnGetAsync()
    {
        var userId = User.GetUserId();
        var resident = await _context.Residents.FirstOrDefaultAsync(r => r.UserId == userId);
        
        if (resident != null)
        {
            Reservations = await _context.FacilityRequests
                .Include(fr => fr.Facility)
                .Where(fr => fr.ResidentId == resident.ResidentId)
                .OrderByDescending(fr => fr.ReservationDate)
                .ThenBy(fr => fr.StartTime)
                .ToListAsync();
        }
        else
        {
            Reservations = new List<FacilityRequest>();
        }
    }
}
