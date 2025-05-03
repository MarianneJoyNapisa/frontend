using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using System.Threading.Tasks;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Admin.Services.ServiceRequests
{
    [Authorize(Roles="admin")]

    public class DetailsModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public ServiceRequest? ServiceRequest { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceRequest = await _context.ServiceRequests
                .Include(sr => sr.Service)
                .Include(sr => sr.Resident)
                .FirstOrDefaultAsync(m => m.ServiceRequestId == id);

            if (ServiceRequest == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}