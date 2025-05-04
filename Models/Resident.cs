using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public class Resident
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

        [Display(Name = "Address")] 
        public string? Address { get; set; } 

        [Display(Name = "Move-In Date")]
        public DateOnly MoveInDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [Display(Name = "Profile Image")]
        public string? ProfileImage { get; set; }

        public virtual User User { get; set; }

        public virtual FacilityReview? FacilityReview { get; set; }

        // public ICollection<ServiceRequest> ServiceRequest { get; set; } = new List<ServiceRequest>();

        public Resident()
        {
            User = new User
            {
                Privilege = Privileges.resident,
                Resident = this
            };
        }
    }
}
