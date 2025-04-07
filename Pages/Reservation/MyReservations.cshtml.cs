using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data;
using System.Security.Claims;

namespace HomeownersMS.Pages.Reservation
{
    public class MyReservationsModel : PageModel
    {
        private readonly HomeownersContext _context;

        public MyReservationsModel(HomeownersContext context)
        {
            _context = context;
        }

        public List<ReservationViewModel> Reservations { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                // Get all facility requests for this resident
                var facilityRequests = await _context.FacilityRequests
                    .Where(fr => fr.ResidentId == userId)
                    .OrderByDescending(fr => fr.RequestDate)
                    .ToListAsync();

                // Get all events for these facility requests
                var facilityRequestIds = facilityRequests.Select(fr => fr.FacilityRequestId).ToList();
                var events = await _context.Events
                    .Where(e => facilityRequestIds.Contains(e.FacilityRequestId ?? 0))
                    .ToListAsync();

                // Join them together for the view
                Reservations = facilityRequests.Select(fr => new ReservationViewModel
                {
                    FacilityRequest = fr,
                    Event = events.FirstOrDefault(e => e.FacilityRequestId == fr.FacilityRequestId) ?? new Event()
                }).ToList();
            }
        }
    }

    public class ReservationViewModel
    {
        public FacilityRequest? FacilityRequest { get; set; }
        public Event? Event { get; set; }
    }
}