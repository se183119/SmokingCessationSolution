using SmokingCessation.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmokingCessation.Data.Entities
{
    public class Achievement
    {
        public int AchievementId { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [MaxLength(1000)]
        public string? Description { get; set; }
        
        public AchievementType Type { get; set; }
        
        public int? RequiredDays { get; set; }
        
        public decimal? RequiredAmount { get; set; }
        
        public int? RequiredCount { get; set; }
        
        [MaxLength(500)]
        public string? IconUrl { get; set; }
        
        [MaxLength(500)]
        public string? BadgeUrl { get; set; }
        
        public int Points { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
    }

    public class UserAchievement
    {
        public int UserAchievementId { get; set; }
        
        public int UserId { get; set; }
        
        public int AchievementId { get; set; }
        
        public DateTime EarnedAt { get; set; }
        
        public bool IsNotified { get; set; } = false;
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
        public virtual Achievement Achievement { get; set; } = null!;
    }
}