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

        public DateOnly? ReservationDate { get; set; } // Date only

        public TimeOnly? ReservationTime { get; set; }  

        public DateTime? RequestCompletionDate { get; set; }

        public Statuses? Status { get; set; }

        [ForeignKey("Resident")]
        public int? ResidentId { get; set; }

        [ForeignKey("Facility")]
        public int? FacilityId { get; set; }
        public string? FullName { get; set; } // Full Name

        public string? EmailAddress { get; set; } // Email Address

        public string? PhoneNumber { get; set; }

        public virtual Resident? Resident { get; set; }
        public virtual Facility? Facility { get; set; }
    }
}
