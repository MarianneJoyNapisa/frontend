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

        public DateTime? RequestedAt { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("Resident")]
        public int? RequestedBy { get; set; }

        [ForeignKey("Service")]
        public int? ServiceId { get; set; }

        public virtual Resident? Resident { get; set; }

        public virtual Service? Service { get; set; }
    }
}
