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

namespace HomeownersMS.Pages.Admin.Services
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly HomeownersContext _context;

        public IndexModel(HomeownersContext context)
        {
            _context = context;
        }

        public IList<Models.Service> Service { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdentity = HttpContext.User.Identity;

            // Fix: Ensure Identity is not null before accessing its properties
            if (userIdentity == null || !userIdentity.IsAuthenticated || !HttpContext.User.IsInRole("admin"))
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            Service = await _context.Services
                .Include(s => s.Staff)
                .ToListAsync();

            return Page();
        }
    }
}
