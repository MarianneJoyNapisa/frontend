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

        public string? DayRange { get; set; } // like Mon-Fri etc.
        public TimeOnly? AvailableTimeStart { get; set; }
        public TimeOnly? AvailableTimeEnd { get; set; }
    }
}

