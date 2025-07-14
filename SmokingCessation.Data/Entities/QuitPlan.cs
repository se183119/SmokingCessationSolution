using SmokingCessation.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmokingCessation.Data.Entities
{
    public class QuitPlan
    {
        public int QuitPlanId { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime? EndDate { get; set; }
        
        public QuitPlanStatus Status { get; set; } = QuitPlanStatus.Draft;
        
        public int? TargetDaysSmokeFree { get; set; }
        
        public decimal? TargetMoneySaved { get; set; }
        
        [MaxLength(500)]
        public string? MotivationalQuote { get; set; }
        
        [MaxLength(1000)]
        public string? PersonalReasons { get; set; }
        
        [MaxLength(1000)]
        public string? TriggerStrategies { get; set; }
        
        [MaxLength(1000)]
        public string? CopingStrategies { get; set; }
        
        [MaxLength(1000)]
        public string? SupportSystem { get; set; }
        
        [MaxLength(1000)]
        public string? RewardSystem { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual ICollection<QuitPlanMilestone> Milestones { get; set; } = new List<QuitPlanMilestone>();
    }
}