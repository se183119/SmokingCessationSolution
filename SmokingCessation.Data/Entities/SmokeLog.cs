using SmokingCessation.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SmokingCessation.Data.Entities
{
    public class SmokeLog
    {
        public int SmokeLogId { get; set; }
        
        public int UserId { get; set; }
        
        public DateTime LogDate { get; set; }
        
        public SmokeLogType LogType { get; set; }
        
        public int? CigarettesSmoked { get; set; }
        
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        [MaxLength(200)]
        public string? Trigger { get; set; }
        
        [MaxLength(200)]
        public string? Location { get; set; }
        
        [MaxLength(200)]
        public string? Mood { get; set; }
        
        public int? CravingIntensity { get; set; } // 1-10 scale
        
        [MaxLength(500)]
        public string? CopingStrategy { get; set; }
        
        public bool SuccessfullyAvoided { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        // Navigation properties
        public virtual User User { get; set; } = null!;
    }
}