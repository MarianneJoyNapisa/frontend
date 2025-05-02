using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using HomeownersMS.Models;
using HomeownersMS.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeownersMS.Pages.Resources
{
    [Authorize(Roles="admin,resident")]
    public class ResourceModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public IList<Resource>? Resources { get; set; }

        public void OnGet()
        {
            Resources = _context.Resources
                .Where(r => r.IsEnabled)
                .OrderByDescending(r => r.CreatedAt)
                .Include(r => r.Admin)
                .ToList();
        }
    }
}
