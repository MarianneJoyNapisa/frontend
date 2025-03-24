using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using HomeownersMS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace HomeownersMS.Pages.Community
{
    [Authorize]
    public class CommunityModel : PageModel
    {
        private readonly Data.HomeownersContext _context;

        public CommunityModel(Data.HomeownersContext context)
        {
            _context = context;
        }

        public List<CommunityPost> Posts { get; set; }

        public async Task OnGetAsync()
        {
            Posts = await _context.CommunityPosts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(string Title, string Content, CommunityPost.Types Type)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Get the current user's ID
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                // Handle the case where user ID is not available or can't be parsed
                return Unauthorized();
            }

            var post = new CommunityPost
            {
                Title = Title,
                Content = Content,
                Type = Type,
                CreatedBy = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.CommunityPosts.Add(post);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}