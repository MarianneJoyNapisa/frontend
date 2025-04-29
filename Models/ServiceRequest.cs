// emptyusing System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public enum Statuses
    {
        pending,
        inProgress,
        completed,
        cancelled
    }
    public class ServiceRequest
    {
        [Key]
        public int ServiceRequestId { get; set; }


        public string? IssueDescription { get; set; }

        public Statuses? Status { get; set; }

        public DateOnly? RequestedDate { get; set; }
        public TimeOnly? RequestedTimeStart { get; set; }
        public TimeOnly? RequestedTimeEnd { get; set; }

        public DateTime? RequestApprovedDateTime { get; set; }
        

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("Resident")]
        public int? RequestedBy { get; set; }

        // General Information Fields

        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        [ForeignKey("Service")]
        public int? ServiceId { get; set; }

        public virtual Resident? Resident { get; set; }

        public virtual Service? Service { get; set; }
    }
}
