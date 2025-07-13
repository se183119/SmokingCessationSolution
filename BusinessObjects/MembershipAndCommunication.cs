using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public class MembershipPackage
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public decimal Price { get; set; }
    
    public int DurationInMonths { get; set; }
    
    [StringLength(1000)]
    public string? Features { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public int SortOrder { get; set; } = 0;
    
    // Navigation properties
    public virtual ICollection<MembershipSubscription> Subscriptions { get; set; } = new List<MembershipSubscription>();
}

public class MembershipSubscription
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int MembershipPackageId { get; set; }
    
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    
    public DateTime EndDate { get; set; }
    
    public decimal AmountPaid { get; set; }
    
    [StringLength(50)]
    public string Status { get; set; } = string.Empty; // Active, Expired, Cancelled
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? CancelledDate { get; set; }
    
    [StringLength(500)]
    public string? CancellationReason { get; set; }
    
    public bool AutoRenew { get; set; } = false;
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
    public virtual MembershipPackage MembershipPackage { get; set; } = null!;
}

public class Consultation
{
    public int Id { get; set; }
    
    public int MemberId { get; set; }
    
    public int? CoachId { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Subject { get; set; } = string.Empty;
    
    [Required]
    [StringLength(2000)]
    public string Question { get; set; } = string.Empty;
    
    [StringLength(2000)]
    public string? Response { get; set; }
    
    [StringLength(50)]
    public string Status { get; set; } = "Pending"; // Pending, Answered, Closed
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? ResponseDate { get; set; }
    
    public int Priority { get; set; } = 1; // 1 = Low, 2 = Medium, 3 = High
    
    [StringLength(50)]
    public string Category { get; set; } = string.Empty; // General, Nutrition, Exercise, Motivation
    
    public bool IsUrgent { get; set; } = false;
    
    // Navigation properties
    public virtual User Member { get; set; } = null!;
    public virtual User? Coach { get; set; }
}

public class Feedback
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [StringLength(100)]
    public string? Title { get; set; }
    
    [Required]
    [StringLength(1000)]
    public string Message { get; set; } = string.Empty;
    
    public int Rating { get; set; } = 5; // 1-5 stars
    
    [StringLength(50)]
    public string Category { get; set; } = string.Empty; // App, Service, Coach, General
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public bool IsPublic { get; set; } = false;
    
    [StringLength(50)]
    public string Status { get; set; } = "New"; // New, Reviewed, Resolved
    
    public DateTime? ReviewedDate { get; set; }
    
    [StringLength(1000)]
    public string? AdminResponse { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; } = null!;
}