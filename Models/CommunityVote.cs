using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models 
{
    public class CommunityVote
{
    [Key]
    public int CommunityVoteId { get; set; }

    [ForeignKey("CommunityPost")]
    public int? CommunityPostId { get; set; }

    [ForeignKey("User")]
    public int? UserId { get; set; }
    public bool? IsUpvote { get; set; } // true for upvote, false for downvote
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    // Navigation properties
    public virtual CommunityPost? Post { get; set; }
    public virtual User? User { get; set; }
}
}