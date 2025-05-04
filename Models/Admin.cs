using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HomeownersMS.Models
{
    public class Admin
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        [Display(Name = "Last Name")]
        public string? LName { get; set; }

        [Display(Name = "First Name")]
        public string? FName { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Display(Name = "Contact No.")]
        public string? ContactNo { get; set; } 

        [Display(Name = "Job")]
        public string? Job { get; set; }

        [Display(Name = "Hired Date")]
        public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Display(Name = "Profile Image")]
        public string? ProfileImage { get; set; }

        public virtual User? User { get; set; }

        public Admin()
        {
            // if (!IsSeeding)
            User = new User
            {
                Privilege = Privileges.admin,
                Admin = this
            };
        }

        // [NotMapped]
        // public static bool IsSeeding { get; set; } = false;
    }
}
