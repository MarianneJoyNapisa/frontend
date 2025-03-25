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
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
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

        // Add this new handler for posting comments
        public async Task<IActionResult> OnPostAddCommentAsync(int postId, string commentContent)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var comment = new CommunityComment
            {
                Content = commentContent,
                CommunityPostId = postId,
                UserId = userId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.CommunityComments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        // Add these new handlers
        public async Task<IActionResult> OnPostEditAsync(int id, string type, string content, string title = null, CommunityPost.Types? postType = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (type == "post")
            {
                var post = await _context.CommunityPosts
                    .FirstOrDefaultAsync(p => p.CommunityPostId == id && p.CreatedBy == userId);

                if (post == null)
                {
                    return NotFound();
                }

                post.Title = title;
                post.Content = content;
                post.Type = postType;
                post.UpdatedAt = DateTime.Now;
            }
            else if (type == "comment")
            {
                var comment = await _context.CommunityComments
                    .FirstOrDefaultAsync(c => c.CommunityCommentId == id && c.UserId == userId);

                if (comment == null)
                {
                    return NotFound();
                }

                comment.Content = content;
                comment.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return new EmptyResult();
        }

        // Add this new handler for deleting posts and comments
        public async Task<IActionResult> OnPostDeleteAsync(int id, string type)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if (type == "post")
            {
                var post = await _context.CommunityPosts
                    .FirstOrDefaultAsync(p => p.CommunityPostId == id && p.CreatedBy == userId);

                if (post == null)
                {
                    return NotFound();
                }

                _context.CommunityPosts.Remove(post);
            }
            else if (type == "comment")
            {
                var comment = await _context.CommunityComments
                    .FirstOrDefaultAsync(c => c.CommunityCommentId == id && c.UserId == userId);

                if (comment == null)
                {
                    return NotFound();
                }

                _context.CommunityComments.Remove(comment);
            }

            await _context.SaveChangesAsync();
            return new EmptyResult();
        }
    }
}