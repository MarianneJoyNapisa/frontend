// emptyusing System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public class CommunityComment
    {
        [Key]
        public int CommunityCommentId { get; set; }

        public string? Content { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey("CommunityPost")]
        public int? CommunityPostId { get; set; }

        [ForeignKey("User")]
        public int? UserId { get; set; }


        public virtual CommunityPost? CommunityPost { get; set; }

        public virtual User? User { get; set; }
    }
}
