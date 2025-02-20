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
    public class DetailsModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public DetailsModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        public Models.Service Service { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var userIdentity = HttpContext.User.Identity;

            // Fix: Ensure Identity is not null before accessing its properties
            if (userIdentity == null || !userIdentity.IsAuthenticated || !HttpContext.User.IsInRole("admin"))
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            if (id == null)
            {
                return NotFound();
            }

            var service = await _context.Services.FirstOrDefaultAsync(m => m.ServiceId == id);
            if (service == null)
            {
                return NotFound();
            }
            else
            {
                Service = service;
            }
            return Page();
        }
    }
}
