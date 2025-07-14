using SmokingCessation.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmokingCessation.Data.Entities
{
    public class QuitPlanMilestone
    {
        public int MilestoneId { get; set; }
        
        public int QuitPlanId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public DateTime TargetDate { get; set; }
        
        public DateTime? CompletedAt { get; set; }
        
        public bool IsCompleted { get; set; } = false;
        
        public int? Points { get; set; }
        
        [MaxLength(500)]
        public string? Reward { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual QuitPlan QuitPlan { get; set; } = null!;
    }

    public class Feedback
    {
        public int FeedbackId { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Subject { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public FeedbackStatus Status { get; set; } = FeedbackStatus.Submitted;
        
        public DateTime SubmittedAt { get; set; }
        
        public DateTime? ResolvedAt { get; set; }
        
        public int? ResolvedByUserId { get; set; }
        
        [MaxLength(2000)]
        public string? Response { get; set; }
        
        public int? Rating { get; set; } // 1-5 star rating
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual User? ResolvedByUser { get; set; }
    }

    public class Template
    {
        public int TemplateId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;
        
        public TemplateType Type { get; set; }
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int CreatedByUserId { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User CreatedByUser { get; set; } = null!;
    }
}