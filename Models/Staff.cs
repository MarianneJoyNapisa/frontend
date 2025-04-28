using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json.Linq;

namespace HomeownersMS.Models
{
    public enum StaffJob
    {
        Maintenance,
        Janitorial
    }
    public class Staff
    {
        [Key, ForeignKey("User")]
        public int UserId { get; set; }
        public string? LName { get; set; }
        public string? FName { get; set; }
        public string? Email { get; set; }
        public string? ContactNo { get; set; }
        public StaffJob? Job { get; set; }
        public DateOnly HireDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public string? ProfileImage { get; set; }
        public virtual User? User { get; set; }

        // Many-to-many to Service
        public ICollection<ServiceStaff> ServiceStaff { get; set; } = new List<ServiceStaff>();

        // public ICollection<ServiceRequest> ServiceRequest { get; set; } = new List<ServiceRequest>();


        public Staff()
        {
            User = new User
            {
                Privilege = Privileges.staff,
                Staff = this
            };
        }
    }
}
