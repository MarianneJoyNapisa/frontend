using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace HomeownersMS.Models
{
    public enum StaffJob
    {
        Maintenance,
        Janitorial,
        Special
    }
    public class Staff
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
        public StaffJob? Job { get; set; }

        [Display(Name = "Hired Date")]
        public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Display(Name = "Profile Image")]
        public string? ProfileImage { get; set; }
        public virtual User? User { get; set; }

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
