using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data; // Ensure this matches your actual namespace


namespace HomeownersMS.Pages.Reservation
{
    public class FunctionHallModel : PageModel
    {
        private readonly HomeownersContext _context;

        public FunctionHallModel(HomeownersContext context)
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


