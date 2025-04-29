using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Authorization;

namespace HomeownersMS.Pages.Admin.Services.ServiceRequests
{
    [Authorize(Roles="admin")]
    public class IndexModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public IList<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();

        public async Task OnGetAsync()
        {
            ServiceRequests = await _context.ServiceRequests
                .Include(sr => sr.Service)
                .Include(sr => sr.Resident)
                .OrderByDescending(sr => sr.CreatedAt)
                .ToListAsync();
        }
    }
}