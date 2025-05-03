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

namespace HomeownersMS.Pages_Admin_Community
{
    [Authorize(Roles="admin")]

    public class DeleteModel : PageModel
    {
        private readonly HomeownersMS.Data.HomeownersContext _context;

        public DeleteModel(HomeownersMS.Data.HomeownersContext context)
        {
            _context = context;
        }

        [BindProperty]
        public CommunityPost CommunityPost { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communitypost = await _context.CommunityPosts.FirstOrDefaultAsync(m => m.CommunityPostId == id);

            if (communitypost == null)
            {
                return NotFound();
            }
            else
            {
                CommunityPost = communitypost;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var communitypost = await _context.CommunityPosts.FindAsync(id);
            if (communitypost != null)
            {
                CommunityPost = communitypost;
                _context.CommunityPosts.Remove(CommunityPost);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
