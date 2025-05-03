using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Admin.Services.ServiceRequests
{
    [Authorize(Roles="admin")]
    public class CreateModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        [BindProperty]
        public ServiceRequest? ServiceRequest { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
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
                _context.ServiceRequests.Add(ServiceRequest);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
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

            ViewData["ServiceId"] = new SelectList(services, "Value", "Text");

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

            ViewData["RequestedBy"] = new SelectList(residents, "Value", "Text");
        }
    }
}