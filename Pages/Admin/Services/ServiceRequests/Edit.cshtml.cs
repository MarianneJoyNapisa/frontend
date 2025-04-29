using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data;
using System.Threading.Tasks;
using System.Linq;

namespace HomeownersMS.Pages.Admin.Services.ServiceRequests
{
    public class EditModel(HomeownersContext context) : PageModel
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

            // Initialize dropdowns
            await InitializeDropdowns();
            
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await InitializeDropdowns();
                return Page();
            }

            if (ServiceRequest != null)
            {
                _context.Attach(ServiceRequest).State = EntityState.Modified;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (ServiceRequest == null || !ServiceRequestExists(ServiceRequest.ServiceRequestId))
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

        private bool ServiceRequestExists(int id)
        {
            return _context.ServiceRequests.Any(e => e.ServiceRequestId == id);
        }

        private async Task InitializeDropdowns()
        {
            // Services dropdown
            var services = await _context.Services
                .OrderBy(s => s.Title)
                .Select(s => new SelectListItem 
                {
                    Value = s.ServiceId.ToString(),
                    Text = s.Title
                })
                .ToListAsync();

            ViewData["ServiceId"] = new SelectList(services, "Value", "Text", ServiceRequest?.ServiceId);

            // Residents dropdown
            var residents = await _context.Residents
                .Include(r => r.User) // Ensure User is loaded
                .OrderBy(r => r.LName)
                .ThenBy(r => r.FName)
                .Select(r => new SelectListItem 
                {
                    Value = r.UserId.ToString(),
                    Text = $"{r.FName} {r.LName}" + 
                           (string.IsNullOrEmpty(r.Email) ? "" : $" ({r.Email})")
                })
                .ToListAsync();

            ViewData["RequestedBy"] = new SelectList(residents, "Value", "Text", ServiceRequest?.RequestedBy);
        }
    }
}