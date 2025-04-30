using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public class Resident
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string? LName { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string? FName { get; set; } 

        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Contact Number is required.")]
        public string? ContactNo { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? Address { get; set; }

        public DateOnly MoveInDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

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
