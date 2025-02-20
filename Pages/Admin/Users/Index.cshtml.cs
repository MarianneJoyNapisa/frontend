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

namespace HomeownersMS.Pages.Admin.Users
{
    [Authorize(Roles = "admin")]
    public class IndexModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public IndexModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        public IList<User> UserList { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdentity = HttpContext.User.Identity;

            // Fix: Ensure Identity is not null before accessing its properties
            if (userIdentity == null || !userIdentity.IsAuthenticated || !HttpContext.User.IsInRole("admin"))
            {
                return RedirectToPage("/Account/AccessDenied");
            }

            UserList = await _context.Users
                .Include(u => u.Admin)
                .Include(u => u.Resident)
                .Include(u => u.Staff).ToListAsync();

            return Page();
        }
    }
}
