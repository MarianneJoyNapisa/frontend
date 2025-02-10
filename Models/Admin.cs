using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HomeownersMS.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

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
        [MaxLength(50)]
        public string Job { get; set; }

        [AllowNull]
        public DateTime? HireDate { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
