using SmokingCessation.Common.Enums;

namespace SmokingCessation.Business.DTOs
{
    public class QuitPlanDto
    {
        public int QuitPlanId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public QuitPlanStatus Status { get; set; }
        public int? TargetDaysSmokeFree { get; set; }
        public decimal? TargetMoneySaved { get; set; }
        public string? MotivationalQuote { get; set; }
        public string? PersonalReasons { get; set; }
        public string? TriggerStrategies { get; set; }
        public string? CopingStrategies { get; set; }
        public string? SupportSystem { get; set; }
        public string? RewardSystem { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<QuitPlanMilestoneDto> Milestones { get; set; } = new List<QuitPlanMilestoneDto>();
        
        // Calculated properties
        public int DaysActive => StartDate <= DateTime.Now ? (DateTime.Now - StartDate).Days : 0;
        public decimal ProgressPercentage => TargetDaysSmokeFree.HasValue && TargetDaysSmokeFree > 0 ? 
            Math.Min(100, (DaysActive * 100.0m) / TargetDaysSmokeFree.Value) : 0;
    }

    public class CreateQuitPlanDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? TargetDaysSmokeFree { get; set; }
        public decimal? TargetMoneySaved { get; set; }
        public string? MotivationalQuote { get; set; }
        public string? PersonalReasons { get; set; }
        public string? TriggerStrategies { get; set; }
        public string? CopingStrategies { get; set; }
        public string? SupportSystem { get; set; }
        public string? RewardSystem { get; set; }
        public List<CreateQuitPlanMilestoneDto> Milestones { get; set; } = new List<CreateQuitPlanMilestoneDto>();
    }

    public class QuitPlanMilestoneDto
    {
        public int MilestoneId { get; set; }
        public int QuitPlanId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsCompleted { get; set; }
        public int? Points { get; set; }
        public string? Reward { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Calculated properties
        public bool IsOverdue => !IsCompleted && TargetDate < DateTime.Now;
        public int DaysRemaining => (TargetDate - DateTime.Now).Days;
    }

    public class CreateQuitPlanMilestoneDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime TargetDate { get; set; }
        public int? Points { get; set; }
        public string? Reward { get; set; }
    }
}