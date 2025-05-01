// Notification.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeownersMS.Models
{
    public class Notification
    {
        [Key]
        public int NotificationId { get; set; }
        
        [Required]
        public string Title { get; set; } = "Default Notification";
        
        [Required]
        public string Message { get; set; } = "This is a simple message.";
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? Url { get; set; }
        
        [ForeignKey("Announcement")]
        public int? AnnouncementId { get; set; }
        public virtual Announcement? Announcement { get; set; }
        
        [ForeignKey("User")]
        public int? CreatedByUserId { get; set; }
        public virtual User? CreatedByUser { get; set; }
    }

    public class UserNotification
    {
        [Key]
        public int UserNotificationId { get; set; }
        
        [ForeignKey("User")]
        public int UserId { get; set; }
        public required virtual User User { get; set; }
        
        [ForeignKey("Notification")]
        public int NotificationId { get; set; }
        public required virtual Notification Notification { get; set; }
        
        public bool IsRead { get; set; } = false;
        public DateTime? ReadAt { get; set; }
    }
}