// emptyusing System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HomeownersMS.Models
{
    public class Announcement
    {
        [Key]
        public int AnnouncementId { get; set; }

        public string? Title { get; set; } 

        public string? Content { get; set; }

        public DateTime? EventDate { get; set;}

        public string? BlocksAffected { get; set; }

        [ForeignKey("Admin")]
        public int CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public virtual Admin? Admin { get; set; }
    }
}
