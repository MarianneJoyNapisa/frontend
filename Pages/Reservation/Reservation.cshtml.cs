using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data;

namespace HomeownersMS.Pages.Reservation
{
    public class ReservationModel : PageModel
    {
        private readonly HomeownersContext _context;

        public ReservationModel(HomeownersContext context)
        {
            _context = context;
        }

        public List<Facility> Facilities { get; set; } = new List<Facility>();

        public async Task OnGetAsync()
        {
            Facilities = await _context.Facilities.ToListAsync();
        }
    }
}