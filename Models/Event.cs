// emptyusing System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace HomeownersMS.Models
{
    public enum EventType
    {
        birthday,
        wedding,
        meeting,
        party,
        workshop,
        others
    }

    public class AdditionalServiceDetails
    {
        public string? Name { get; set; }
        public int? Price { get; set; }
        public string? Description { get; set; }
    }

    namespace HomeownersMS.Models
    {
        public static class PredefinedServices
        {
            public static readonly AdditionalServiceDetails AirConditioning = new AdditionalServiceDetails
            {
                Name = "Air Conditioning",
                Price = 200,
                Description = "Super cool air conditioning"
            };

            public static readonly AdditionalServiceDetails TableAndChairs = new AdditionalServiceDetails
            {
                Name = "Table and Chairs",
                Price = 100,
                Description = "Comfortable tables and chairs"
            };

            public static readonly AdditionalServiceDetails SoundSystem = new AdditionalServiceDetails
            {
                Name = "Sound System",
                Price = 150,
                Description = "High-quality sound system"
            };

            public static readonly AdditionalServiceDetails ProjectorAndScreen = new AdditionalServiceDetails
            {
                Name = "Projector and Screen",
                Price = 250,
                Description = "HD projector and screen for presentations"
            };

            public static readonly AdditionalServiceDetails Decorations = new AdditionalServiceDetails
            {
                Name = "Decorations",
                Price = 300,
                Description = "Beautiful decorations for your event"
            };
        }
    }

    public class Event
    {
        [Key]
        public int EventId { get; set; }

        public string? Title { get; set; }

        public EventType EventType { get; set; }

        public DateOnly? EventDate { get; set; }
        public TimeOnly? EventTimeStart { get; set; }

        public TimeOnly? EventTimeEnd { get; set; }

        public int? GuestCapacity { get; set; }

        // Contact info

        public int? ContactNumber { get; set; }

        public string? ContactEmail { get; set; }

        public string? ContactName { get; set; }

        public Dictionary<string, AdditionalServiceDetails> AdditionalServices { get; set; } = new();
        /* 
            example of creating new event
                    var newEvent = new Event
                    {
                        Title = "Wedding Celebration",
                        EventType = EventType.wedding,
                        EventDate = new DateOnly(2025, 5, 20),
                        EventTimeStart = new TimeOnly(15, 0),
                        EventTimeEnd = new TimeOnly(20, 0),
                        GuestCapacity = 100,
                        ContactName = "John Doe",
                        ContactNumber = 1234567890,
                        ContactEmail = "john.doe@example.com",
                        AdditionalServices = new Dictionary<string, AdditionalServiceDetails>
                        {
                            { "AirConditioning", PredefinedServices.AirConditioning },
                            { "Decorations", PredefinedServices.Decorations }
                        },
                        CreatedBy = 1
                    };
        */

        [ForeignKey("User")]
        public int? CreatedBy { get; set; }

        [ForeignKey("FacilityRequest")]
        public int? FacilityRequestId { get; set;}

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual User? User { get; set; }

        public virtual FacilityRequest? FacilityRequest { get; set;}
    }
}
