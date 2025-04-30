using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using HomeownersMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Staff.Services
{
    [Authorize(Roles = "staff,admin")]
    public class RequestsModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public Models.Service? Service { get; set; }
        public List<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();

        public async Task<IActionResult> OnGetAsync(int serviceId)
        {
            Service = await _context.Services
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId);

            if (Service == null)
            {
                return NotFound();
            }

            ServiceRequests = await _context.ServiceRequests
                .Include(sr => sr.Resident)
                .Where(sr => sr.ServiceId == serviceId && sr.Status == Statuses.pending)
                .OrderByDescending(sr => sr.RequestedDate)
                .ToListAsync();

            return Page();
        }
    }
}