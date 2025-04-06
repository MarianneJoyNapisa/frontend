using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models // ← ADD THIS
{
    public enum RequestStatus
    {
        Pending,
        Approved,
        Declined
    }

    public class FacilityRequest
    {
        [Key]
        public int FacilityRequestId { get; set; }

        [Required]
        public DateOnly ReservationDate { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;
        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        [ForeignKey("Resident")]
        public int ResidentId { get; set; }

        [ForeignKey("Facility")]
        public int FacilityId { get; set; }

        public string? AdminNotes { get; set; }

        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Resident Resident { get; set; }
        public virtual Facility Facility { get; set; }
    }
}
