using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeownersMS.Models;
using HomeownersMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeownersMS.Pages.Settings
{
    [Authorize]
    public class IndexModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}