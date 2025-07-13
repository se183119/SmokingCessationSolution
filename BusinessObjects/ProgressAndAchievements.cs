using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public class ProgressTracking
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public DateTime RecordDate { get; set; } = DateTime.UtcNow.Date;
    
    public int CigarettesSmoked { get; set; } = 0;
    
    public decimal MoneySaved { get; set; } = 0;
    
    public decimal MoneySpent { get; set; } = 0;
    
    public int CravingsCount { get; set; } = 0;
    
    [StringLength(10)]
    public string MoodLevel { get; set; } = string.Empty; // Excellent, Good, Fair, Poor
    
    [StringLength(10)]
    public string EnergyLevel { get; set; } = string.Empty; // High, Medium, Low
    
    [StringLength(10)]
    public string SleepQuality { get; set; } = string.Empty; // Excellent, Good, Fair, Poor
    
    public decimal Weight { get; set; } = 0;
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    [StringLength(1000)]
    public string? Challenges { get; set; }
    
    [StringLength(1000)]
    public string? Successes { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}

public class Achievement
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    [StringLength(50)]
    public string Category { get; set; } = string.Empty; // Days Smoke-Free, Money Saved, Health Improvement
    
    [StringLength(500)]
    public string? IconPath { get; set; }
    
    public int PointsAwarded { get; set; } = 0;
    
    public DateTime AchievedDate { get; set; } = DateTime.UtcNow;
    
    [StringLength(50)]
    public string BadgeLevel { get; set; } = string.Empty; // Bronze, Silver, Gold, Platinum
    
    public bool IsVisible { get; set; } = true;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}

public class Notification
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string Type { get; set; } = string.Empty; // Motivational, Achievement, Reminder, System
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? ReadDate { get; set; }
    
    public bool IsRead { get; set; } = false;
    
    public bool IsActive { get; set; } = true;
    
    [StringLength(500)]
    public string? ActionUrl { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}