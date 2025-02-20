using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace HomeownersMS.Models
{
    public class Staff
    {
        [Key]
        public int StaffId { get; set; }

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
        public virtual ICollection<Service> Services { get; set; } = new List<Service>();

        public Staff()
        {
            User = new User
            {
                Privilege = Privileges.staff,
                Staff = this
            };
        }
    }
}
