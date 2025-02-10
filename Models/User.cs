using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public enum Privileges
    {
        resident, staff, admin
    }
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MaxLength(255)]
        public string PasswordHash {  get; set; }

        [Required]
        public Privileges Privilege { get; set; }

        public int? ResidentId { get; set; }
        public int? StaffId { get; set; }
        public int? AdminId { get; set; }

        [ForeignKey("ResidentId")]
        public virtual Resident? Resident { get; set; }

        [ForeignKey("StaffId")]
        public virtual Staff? Staff { get; set; }

        [ForeignKey("AdminId")]
        public virtual Admin? Admin { get; set; }

        public void SetPassword(string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            PasswordHash = passwordHasher.HashPassword(this, password);
        }

        public bool VerifyPassword(string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(this, PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
