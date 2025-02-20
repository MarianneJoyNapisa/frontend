using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HomeownersMS.Models
{
    public class Admin
    {
        [Key]
        public int AdminId { get; set; }

        [AllowNull]
        [MaxLength(50)]
        public string? LName { get; set; } = string.Empty;

        [AllowNull]
        [MaxLength(50)]
        public string? FName { get; set; } = string.Empty;

        [AllowNull]
        [MaxLength(50)]
        public string? Email { get; set; } = string.Empty;

        [AllowNull]
        [MaxLength(50)]
        public string? ContactNo { get; set; } = string.Empty;

        [AllowNull]
        [MaxLength(50)]
        public string? Job { get; set; } = string.Empty;

        [AllowNull]
        public DateTime? HireDate { get; set; }

        public virtual User User { get; set; }

        public Admin()
        {
            if (!IsSeeding)
            {
                User = new User
                {
                    Privilege = Privileges.admin,
                    Admin = this
                };
            }
        }

        [NotMapped]
        public static bool IsSeeding { get; set; } = false;
    }
}
