using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public enum ServiceCategory {
        repairsAndMaintenance,
        cleaningServices,
        landscapingMaintenance,
        otherServices
    }
    public class Service 
    {
        [Key]
        public int ServiceId { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public ServiceCategory? ServiceCategory { get; set; }

        public DateTime? AvailableDateTimeStart { get; set; }
        public DateTime? AvailableDateTimeEnd { get; set; }

        // Many-to-many to Staff
        public ICollection<ServiceStaff> ServiceStaff { get; set; } = new List<ServiceStaff>();

        // public ICollection<ServiceRequest> ServiceRequest { get; set; } = new List<ServiceRequest>();
    }
}

