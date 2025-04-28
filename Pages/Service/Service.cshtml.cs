using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Data;

namespace HomeownersMS.Pages.Service
{
    public class ServiceModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public List<Models.Service> Services { get; set; } = new List<Models.Service>();
        public List<ServiceRequest> CurrentRequests { get; set; } = new List<ServiceRequest>();
        public List<ServiceRequest> HistoricalRequests { get; set; } = new List<ServiceRequest>();

        public async Task OnGetAsync()
        {
            // Get services from database
            Services = await _context.Services
                .Include(s => s.ServiceStaff)
                .ToListAsync();

            // Get current requests (pending or in progress)
            CurrentRequests = await _context.ServiceRequests
                .Include(r => r.Service)
                .Where(r => r.Status == Statuses.pending || r.Status == Statuses.inProgress)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            // Get historical requests (completed or cancelled)
            HistoricalRequests = await _context.ServiceRequests
                .Include(r => r.Service)
                .Where(r => r.Status == Statuses.completed || r.Status == Statuses.cancelled)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }
    }
}