using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public class ServiceStaff
    {
        [Key]
        public int ServiceStaffId { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public int StaffId { get; set; }
        public Staff? Staff { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}

// Many-to-many middle table between Service and Staff