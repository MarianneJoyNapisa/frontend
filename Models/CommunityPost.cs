// emptyusing System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public class CommunityPost
    {
        public enum Types
        {
            concern, suggestion, general, advice
        }

        [Key]
        public int CommunityPostId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Types? Type { get; set; }
        [ForeignKey("User")]
        public int? CreatedBy { get; set; }

        public int Vote { get; set; } = 0;


        public virtual User? User { get; set; }

        public ICollection<CommunityComment> Comments { get; set; } = new List<CommunityComment>();

        public virtual ICollection<CommunityVote> Votes { get; set; } = new List<CommunityVote>();
    }
}
