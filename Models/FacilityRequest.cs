using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Facility Request or Facility Reservation

namespace HomeownersMS.Models
{
    public class FacilityRequest
    {
        [Key]
        public int FacilityRequestId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ReservationDate { get; set; } // Date only

        [DataType(DataType.Time)]
        public TimeSpan? ReservationTime { get; set; }  

        public DateTime? RequestCompletionDate { get; set; }

        public Statuses? Status { get; set; }

        [ForeignKey("Resident")]
        public int? ResidentId { get; set; }

        [ForeignKey("Facility")]
        public int? FacilityId { get; set; }

        [Required]
        public string? FullName { get; set; } // Full Name

        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; } // Email Address

        [Required]
        [Phone]
        public string? PhoneNumber { get; set; }

        public virtual Resident? Resident { get; set; }
        public virtual Facility? Facility { get; set; }
    }
}
