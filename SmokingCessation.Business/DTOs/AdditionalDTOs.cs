using SmokingCessation.Common.Enums;

namespace SmokingCessation.Business.DTOs
{
    public class AchievementDto
    {
        public int AchievementId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public AchievementType Type { get; set; }
        public int? RequiredDays { get; set; }
        public decimal? RequiredAmount { get; set; }
        public int? RequiredCount { get; set; }
        public string? IconUrl { get; set; }
        public string? BadgeUrl { get; set; }
        public int Points { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEarned { get; set; }
        public DateTime? EarnedAt { get; set; }
        
        public string TypeDescription => Type switch
        {
            AchievementType.TimeBasedMilestone => "Time Milestone",
            AchievementType.MoneyBasedMilestone => "Money Milestone",
            AchievementType.HealthBasedMilestone => "Health Milestone",
            AchievementType.BehaviorBasedMilestone => "Behavior Milestone",
            _ => "Unknown"
        };
    }

    public class MessageDto
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public MessageStatus Status { get; set; }
        public DateTime SentAt { get; set; }
        public DateTime? ReadAt { get; set; }
        public bool IsDeleted { get; set; }
        public int? ParentMessageId { get; set; }
        public List<MessageDto> Replies { get; set; } = new List<MessageDto>();
        
        public bool IsRead => ReadAt.HasValue;
        public string TimeAgo => (DateTime.Now - SentAt).TotalDays > 1 
            ? SentAt.ToString("MMM dd, yyyy")
            : SentAt.ToString("HH:mm");
    }

    public class SendMessageDto
    {
        public int ReceiverId { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? ParentMessageId { get; set; }
    }

    public class CoachDto
    {
        public int CoachId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Specialization { get; set; }
        public string? Bio { get; set; }
        public string? Qualifications { get; set; }
        public int YearsOfExperience { get; set; }
        public CoachStatus Status { get; set; }
        public DateTime AppliedAt { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int? ApprovedByUserId { get; set; }
        public decimal? HourlyRate { get; set; }
        public bool IsAvailable { get; set; }
        public int MaxMembers { get; set; }
        public int CurrentMemberCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string StatusDescription => Status switch
        {
            CoachStatus.Pending => "Pending Approval",
            CoachStatus.Approved => "Approved",
            CoachStatus.Rejected => "Rejected",
            CoachStatus.Suspended => "Suspended",
            _ => "Unknown"
        };
    }

    public class ApplyAsCoachDto
    {
        public string? Specialization { get; set; }
        public string? Bio { get; set; }
        public string? Qualifications { get; set; }
        public int YearsOfExperience { get; set; }
        public decimal? HourlyRate { get; set; }
        public int MaxMembers { get; set; } = 20;
    }

    public class FeedbackDto
    {
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public FeedbackStatus Status { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public int? ResolvedByUserId { get; set; }
        public string? ResolvedByUserName { get; set; }
        public string? Response { get; set; }
        public int? Rating { get; set; }
        
        public string StatusDescription => Status switch
        {
            FeedbackStatus.Submitted => "Submitted",
            FeedbackStatus.InReview => "In Review",
            FeedbackStatus.Resolved => "Resolved",
            FeedbackStatus.Closed => "Closed",
            _ => "Unknown"
        };
    }

    public class SubmitFeedbackDto
    {
        public string Subject { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int? Rating { get; set; }
    }
}