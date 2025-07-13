using System.ComponentModel.DataAnnotations;

namespace BusinessObjects;

public enum UserRole
{
    Guest = 0,
    Member = 1,
    Coach = 2,
    Admin = 3
}

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;
    
    public UserRole Role { get; set; } = UserRole.Guest;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? LastLoginDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    [StringLength(15)]
    public string? PhoneNumber { get; set; }
    
    public DateTime? DateOfBirth { get; set; }
    
    [StringLength(10)]
    public string? Gender { get; set; }
    
    [StringLength(500)]
    public string? ProfilePicturePath { get; set; }
    
    // Navigation properties
    public virtual ICollection<SmokingStatus> SmokingStatuses { get; set; } = new List<SmokingStatus>();
    public virtual ICollection<QuitPlan> QuitPlans { get; set; } = new List<QuitPlan>();
    public virtual ICollection<ProgressTracking> ProgressTrackings { get; set; } = new List<ProgressTracking>();
    public virtual ICollection<Achievement> Achievements { get; set; } = new List<Achievement>();
    public virtual ICollection<Consultation> ConsultationsAsMember { get; set; } = new List<Consultation>();
    public virtual ICollection<Consultation> ConsultationsAsCoach { get; set; } = new List<Consultation>();
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public virtual MembershipSubscription? MembershipSubscription { get; set; }
}
