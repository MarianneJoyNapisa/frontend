using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data;

namespace HomeownersMS.Pages.Admin.Reservations
{
    [Authorize(Roles = "Admin")]
    public class ManageReservationsModel : PageModel
    {
        private readonly HomeownersContext _context;

        public ManageReservationsModel(HomeownersContext context)
        {
            _context = context;
        }

        public List<FacilityRequest> Reservations { get; set; }

        public async Task OnGetAsync()
        {
            Reservations = await _context.FacilityRequests
                .Include(fr => fr.Facility)
                .Include(fr => fr.Resident)
                .OrderBy(fr => fr.ReservationDate)
                .ThenBy(fr => fr.StartTime)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostApproveAsync(int id)
        {
            var request = await _context.FacilityRequests.FindAsync(id);
            if (request != null)
            {
                request.Status = RequestStatus.Approved;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeclineAsync(int id, string adminNotes)
        {
            var request = await _context.FacilityRequests.FindAsync(id);
            if (request != null)
            {
                request.Status = RequestStatus.Declined;
                request.AdminNotes = adminNotes;
                await _context.SaveChangesAsync();
            }
            return RedirectToPage();
        }
    }
}