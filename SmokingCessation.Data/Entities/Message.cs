using SmokingCessation.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmokingCessation.Data.Entities
{
    public class Message
    {
        public int MessageId { get; set; }
        
        public int SenderId { get; set; }
        
        public int ReceiverId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public MessageStatus Status { get; set; } = MessageStatus.Sent;
        
        public DateTime SentAt { get; set; }
        
        public DateTime? ReadAt { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        
        public int? ParentMessageId { get; set; }
        
        // Navigation properties
        public virtual User Sender { get; set; } = null!;
        public virtual User Receiver { get; set; } = null!;
        public virtual Message? ParentMessage { get; set; }
        public virtual ICollection<Message> Replies { get; set; } = new List<Message>();
    }

    public class Coach
    {
        public int CoachId { get; set; }
        
        public int UserId { get; set; }
        
        [MaxLength(1000)]
        public string? Specialization { get; set; }
        
        [MaxLength(2000)]
        public string? Bio { get; set; }
        
        [MaxLength(1000)]
        public string? Qualifications { get; set; }
        
        public int YearsOfExperience { get; set; }
        
        public CoachStatus Status { get; set; } = CoachStatus.Pending;
        
        public DateTime AppliedAt { get; set; }
        
        public DateTime? ApprovedAt { get; set; }
        
        public int? ApprovedByUserId { get; set; }
        
        public decimal? HourlyRate { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        public int MaxMembers { get; set; } = 20;
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual User? ApprovedByUser { get; set; }
        public virtual ICollection<User> AssignedMembers { get; set; } = new List<User>();
    }
}