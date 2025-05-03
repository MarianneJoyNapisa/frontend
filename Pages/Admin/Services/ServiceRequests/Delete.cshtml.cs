using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using System.Threading.Tasks;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Admin.Services.ServiceRequests
{
    [Authorize(Roles="admin")]

    public class DeleteModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ServiceRequest = await _context.ServiceRequests.FindAsync(id);

            if (ServiceRequest != null)
            {
                _context.ServiceRequests.Remove(ServiceRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}