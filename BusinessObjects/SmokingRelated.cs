using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public class SmokingStatus
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int CigarettesPerDay { get; set; }
    
    public decimal CostPerPack { get; set; }
    
    public int CigarettesPerPack { get; set; } = 20;
    
    [StringLength(50)]
    public string SmokingFrequency { get; set; } = string.Empty; // Daily, Weekly, Occasionally
    
    public int YearsSmoked { get; set; }
    
    [StringLength(100)]
    public string? PreferredBrand { get; set; }
    
    public DateTime RecordedDate { get; set; } = DateTime.UtcNow;
    
    [StringLength(500)]
    public string? Notes { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Calculated properties
    public decimal DailyCost => (decimal)CigarettesPerDay / CigarettesPerPack * CostPerPack;
    public decimal MonthlyCost => DailyCost * 30;
    public decimal YearlyCost => DailyCost * 365;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}

public class QuitPlan
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(1000)]
    public string? Description { get; set; }
    
    public DateTime QuitDate { get; set; }
    
    [StringLength(50)]
    public string PlanType { get; set; } = string.Empty; // Gradual, Cold Turkey, Nicotine Replacement
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? CompletedDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    [StringLength(1000)]
    public string? Motivations { get; set; }
    
    [StringLength(1000)]
    public string? Triggers { get; set; }
    
    [StringLength(1000)]
    public string? CopingStrategies { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual ICollection<QuitPlanMilestone> Milestones { get; set; } = new List<QuitPlanMilestone>();
}

public class QuitPlanMilestone
{
    public int Id { get; set; }
    
    public int QuitPlanId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public int DaysFromQuit { get; set; }
    
    public DateTime? AchievedDate { get; set; }
    
    public bool IsAchieved { get; set; } = false;
    
    [StringLength(500)]
    public string? Reward { get; set; }
    
    // Navigation properties
    public virtual QuitPlan QuitPlan { get; set; } = null!;
}