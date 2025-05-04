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
    public class CommunityModel(Data.HomeownersContext context) : PageModel
    {
        private readonly Data.HomeownersContext _context = context;

        public List<CommunityPost>? Posts { get; set; }

        public async Task OnGetAsync(string searchTerm = "", string sortBy = "newest")
        {
            IQueryable<CommunityPost> query = _context.CommunityPosts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Votes);

            // Apply search filter if search term exists
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(p => 
                    (p.Title != null && p.Title.ToLower().Contains(searchTerm)) ||
                    (p.Content != null && p.Content.ToLower().Contains(searchTerm)) ||
                    (p.User != null && p.User.Username != null && p.User.Username.ToLower().Contains(searchTerm)));
            }

            // Apply sorting
            query = sortBy switch
            {
                "oldest" => query.OrderBy(p => p.CreatedAt),
                "most-commented" => query.OrderByDescending(p => p.Comments.Count),
                "most-voted" => query.OrderByDescending(p => p.Votes.Count(v => v.IsUpvote == true) - 
                                p.Votes.Count(v => v.IsUpvote == false)),
                _ => query.OrderByDescending(p => p.CreatedAt) // Default: newest first
            };

            Posts = await query.ToListAsync();
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

        // Voting handler
        public async Task<IActionResult> OnPostVoteAsync(int postId, bool? isUpvote)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            // Check if user already voted on this post
            var existingVote = await _context.CommunityVotes
                .FirstOrDefaultAsync(v => v.CommunityPostId == postId && v.UserId == userId);

            if (existingVote != null)
            {
                if (existingVote.IsUpvote == isUpvote)
                {
                    // User is clicking the same vote again - remove the vote
                    _context.CommunityVotes.Remove(existingVote);
                }
                else
                {
                    // User is changing their vote
                    existingVote.IsUpvote = isUpvote;
                    existingVote.CreatedAt = DateTime.Now;
                }
            }
            else if (isUpvote.HasValue)
            {
                // New vote
                var vote = new CommunityVote
                {
                    CommunityPostId = postId,
                    UserId = userId,
                    IsUpvote = isUpvote,
                    CreatedAt = DateTime.Now
                };
                _context.CommunityVotes.Add(vote);
            }

            await _context.SaveChangesAsync();

            // Return the updated vote counts
            var post = await _context.CommunityPosts
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.CommunityPostId == postId);

            if (post == null)
            {
                return NotFound();
            }

            // Calculate and update the post's vote count (upvotes - downvotes)
            post.Vote = post.Votes.Count(v => v.IsUpvote == true) - post.Votes.Count(v => v.IsUpvote == false);
            await _context.SaveChangesAsync();

            return new JsonResult(new { 
                upvotes = post.Votes.Count(v => v.IsUpvote == true),
                downvotes = post.Votes.Count(v => v.IsUpvote == false),
                userVote = existingVote?.IsUpvote
            });
        }

        // Add this new handler for posting comments
        public async Task<IActionResult> OnPostAddCommentAsync(int postId, string commentContent)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
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
        public async Task<IActionResult> OnPostEditAsync(int id, string type, string content, string? title = null, CommunityPost.Types? postType = null)
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

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
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

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