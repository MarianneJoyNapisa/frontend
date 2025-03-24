using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HomeownersMS.Data;
using HomeownersMS.Models;

namespace HomeownersMS.Pages_Admin_Community
{
    public class CreateModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public CreateModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CreatedBy"] = new SelectList(_context.Users, "UserId", "UserId");
            return Page();
        }

        [BindProperty]
        public CommunityPost CommunityPost { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.CommunityPosts.Add(CommunityPost);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
