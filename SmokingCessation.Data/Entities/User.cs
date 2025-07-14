using SmokingCessation.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmokingCessation.Data.Entities
{
    public class User
    {
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? FirstName { get; set; }
        
        [MaxLength(100)]
        public string? LastName { get; set; }
        
        public DateTime? DateOfBirth { get; set; }
        
        [MaxLength(20)]
        public string? Phone { get; set; }
        
        public UserRole Role { get; set; } = UserRole.Member;
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [MaxLength(500)]
        public string? ProfilePicture { get; set; }
        
        public DateTime? SmokingQuitDate { get; set; }
        
        public int? CigarettesPerDay { get; set; }
        
        public decimal? CostPerPack { get; set; }
        
        public int? CigarettesPerPack { get; set; }
        
        // Navigation properties
        public virtual ICollection<QuitPlan> QuitPlans { get; set; } = new List<QuitPlan>();
        public virtual ICollection<SmokeLog> SmokeLogs { get; set; } = new List<SmokeLog>();
        public virtual ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
        public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
        public virtual Coach? Coach { get; set; }
        public virtual ICollection<User> AssignedMembers { get; set; } = new List<User>();
    }
}