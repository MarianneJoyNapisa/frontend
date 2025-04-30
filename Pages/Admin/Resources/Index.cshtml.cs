using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HomeownersMS.Models;
using HomeownersMS.Data;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HomeownersMS.Pages.Admin.Resources
{
    public class IndexModel(HomeownersContext context) : PageModel
    {
        private readonly HomeownersContext _context = context;

        public IList<Resource> Resources { get; set; } = new List<Resource>();

        public async Task OnGetAsync()
        {
            Resources = await _context.Resources
                .Include(r => r.Admin)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostToggleStatusAsync(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            resource.IsEnabled = !resource.IsEnabled;
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}