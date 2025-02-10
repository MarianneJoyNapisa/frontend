using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace HomeownersMS.Models
{
    public class Resident
    {
        [Key]
        public int ResidentId { get; set; }

        [Required]
        [MaxLength(50)]
        public string LName { get; set; }

        [Required]
        [MaxLength(50)]
        public string FName { get; set; }

        [AllowNull]
        [MaxLength(50)]
        public string Email { get; set; }

        [AllowNull]
        [MaxLength(50)]
        public string ContactNo { get; set; }

        [AllowNull]
        [MaxLength(255)]
        public string Address { get; set; }

        [AllowNull]
        public DateTime? MoveInDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
