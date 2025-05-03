using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HomeownersMS.Pages.Service
{
    [Authorize(Roles = "admin,resident")]
    public class ServiceRequestModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        [BindProperty]
        public ServiceRequest ServiceRequest { get; set; } = new ServiceRequest();

        public string? ServiceTitle { get; set; }
        public string? ServiceCategory { get; set; }
        public Resident? Resident { get; set; }

        public async Task<IActionResult> OnGetAsync(int serviceId)
        {
            // Get the service details
            var service = await _context.Services
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

            if (service == null)
            {
                return NotFound();
            }

            ServiceTitle = service.Title;
            ServiceCategory = service.ServiceCategory.ToString();
            ServiceRequest.ServiceId = serviceId;

            // Get resident details if logged in
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                Resident = await _context.Residents
                    .FirstOrDefaultAsync(r => r.UserId == int.Parse(userId));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload service details for display
                var service = await _context.Services
                    .FirstOrDefaultAsync(s => s.ServiceId == ServiceRequest.ServiceId);
                
                if (service != null)
                {
                    ServiceTitle = service.Title;
                    ServiceCategory = service.ServiceCategory.ToString();
                }

                return Page();
            }

            // Set additional fields
            ServiceRequest.Status = Statuses.pending;
            ServiceRequest.StaffAcceptedBy = null;
            ServiceRequest.CreatedAt = DateTime.Now;
            
            // Set resident ID if logged in
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                var resident = await _context.Residents
                    .FirstOrDefaultAsync(r => r.UserId == int.Parse(userId));
                
                if (resident != null)
                {
                    ServiceRequest.RequestedBy = resident.UserId;
                }
            }

            Console.WriteLine("\nStatus: " + ServiceRequest.Status);
            Console.WriteLine("StaffAcceptedBy: " + ServiceRequest.StaffAcceptedBy);
            Console.WriteLine("RequestedBy: " + ServiceRequest.RequestedBy);

            _context.ServiceRequests.Add(ServiceRequest);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Service/Service", new { success = true });
        }
    }
}