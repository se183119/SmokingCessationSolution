using SmokingCessation.Common.Enums;

namespace SmokingCessation.Business.DTOs
{
    public class SmokeLogDto
    {
        public int SmokeLogId { get; set; }
        public int UserId { get; set; }
        public DateTime LogDate { get; set; }
        public SmokeLogType LogType { get; set; }
        public int? CigarettesSmoked { get; set; }
        public string? Notes { get; set; }
        public string? Trigger { get; set; }
        public string? Location { get; set; }
        public string? Mood { get; set; }
        public int? CravingIntensity { get; set; }
        public string? CopingStrategy { get; set; }
        public bool SuccessfullyAvoided { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // Calculated properties
        public string LogTypeDescription => LogType switch
        {
            SmokeLogType.Smoked => "Smoked",
            SmokeLogType.Craving => "Craving",
            SmokeLogType.Alternative => "Alternative Action",
            _ => "Unknown"
        };
        
        public string TimeAgo => (DateTime.Now - LogDate).TotalDays > 1 
            ? LogDate.ToString("MMM dd, yyyy")
            : LogDate.ToString("HH:mm");
    }

    public class CreateSmokeLogDto
    {
        public DateTime LogDate { get; set; } = DateTime.Now;
        public SmokeLogType LogType { get; set; }
        public int? CigarettesSmoked { get; set; }
        public string? Notes { get; set; }
        public string? Trigger { get; set; }
        public string? Location { get; set; }
        public string? Mood { get; set; }
        public int? CravingIntensity { get; set; }
        public string? CopingStrategy { get; set; }
        public bool SuccessfullyAvoided { get; set; }
    }

    public class DashboardStatsDto
    {
        public int DaysSmokeFree { get; set; }
        public decimal MoneySaved { get; set; }
        public int CigarettesAvoided { get; set; }
        public TimeSpan TimeSaved { get; set; }
        public int TotalLogs { get; set; }
        public int CravingsOvercome { get; set; }
        public int AchievementsEarned { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public List<WeeklySummaryDto> WeeklySummary { get; set; } = new List<WeeklySummaryDto>();
    }

    public class WeeklySummaryDto
    {
        public DateTime WeekStart { get; set; }
        public DateTime WeekEnd { get; set; }
        public int SmokeDays { get; set; }
        public int SmokeFreedays { get; set; }
        public int TotalCigarettes { get; set; }
        public int CravingsLogged { get; set; }
        public decimal WeeklyMoneySaved { get; set; }
    }

    public class ProgressChartDto
    {
        public List<DailyProgressDto> DailyProgress { get; set; } = new List<DailyProgressDto>();
        public List<MoodTrendDto> MoodTrends { get; set; } = new List<MoodTrendDto>();
        public List<TriggerAnalysisDto> TriggerAnalysis { get; set; } = new List<TriggerAnalysisDto>();
    }

    public class DailyProgressDto
    {
        public DateTime Date { get; set; }
        public int CigarettesSmoked { get; set; }
        public int CravingsLogged { get; set; }
        public bool WasSmokeFree { get; set; }
        public decimal DailyMoneySaved { get; set; }
    }

    public class MoodTrendDto
    {
        public string Mood { get; set; } = string.Empty;
        public int Count { get; set; }
        public double AverageCravingIntensity { get; set; }
    }

    public class TriggerAnalysisDto
    {
        public string Trigger { get; set; } = string.Empty;
        public int Count { get; set; }
        public int SuccessfulAvoidances { get; set; }
        public double SuccessRate { get; set; }
    }
}