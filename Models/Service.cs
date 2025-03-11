using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public class Service 
    {
        [Key]
        public int ServiceId { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        [ForeignKey("Staff")]
        public int? StaffId { get; set; }

        public virtual Staff? Staff { get; set; }

        public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new List<ServiceRequest>();
    }
}

