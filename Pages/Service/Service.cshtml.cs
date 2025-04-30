using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeownersMS.Pages.Service
{
    [Authorize(Roles="admin,resident")]
    public class ServiceModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public List<Models.Service> Services { get; set; } = new List<Models.Service>();
        public List<ServiceRequest> CurrentRequests { get; set; } = new List<ServiceRequest>();
        public List<ServiceRequest> HistoricalRequests { get; set; } = new List<ServiceRequest>();
        
        // Pagination properties
        public async Task OnGetAsync(bool? success)
        {

            // Get services from database
            Services = await _context.Services
                .ToListAsync();

            // Get current requests (pending or in progress)
            CurrentRequests = await _context.ServiceRequests
                .Include(r => r.Service)
                .Where(r => r.Status == Statuses.pending || r.Status == Statuses.inProgress)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            // Get historical requests (completed or cancelled) with pagination
            HistoricalRequests = await _context.ServiceRequests
                .Include(r => r.Service)
                .Where(r => r.Status == Statuses.completed || r.Status == Statuses.cancelled)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            if (success == true)
            {
                ViewData["SuccessMessage"] = "Your service request has been submitted successfully!";
            }
        }

        public async Task<IActionResult> OnPostMarkAsCompletedAsync(int serviceRequestId)
        {
            var request = await _context.ServiceRequests
                .FirstOrDefaultAsync(r => r.ServiceRequestId == serviceRequestId);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = Statuses.completed;
            request.RequestApprovedDateTime = DateTime.Now;
            
            await _context.SaveChangesAsync();

            return RedirectToPage(new { success = true });
        }
    }
}