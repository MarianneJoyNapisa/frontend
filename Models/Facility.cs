using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public class Facility
    {
        [Key]
        public int FacilityId { get; set; }

        // These properties will be mapped to TEXT in SQLite
        public string? Name { get; set; }

        public string? Description { get; set; }

        // This will be mapped to REAL in SQLite
        public float? PricePerHour { get; set; }

        // Facility Image (stored as a file path or URL)
        public string? FacilityImage { get; set; }

        public string? Address { get; set;}

        // Navigation property for reviews
        public virtual ICollection<FacilityReview> FacilityReviews { get; set; } = new List<FacilityReview>();
    }
}
