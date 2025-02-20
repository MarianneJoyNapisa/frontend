using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public class Service 
    {
        [Key]
        public int ServiceId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int? StaffId { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff Staff { get; set; } = null!;
    }
}

