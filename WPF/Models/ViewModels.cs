namespace WPF.Models;

public class DashboardData
{
    public int DaysSmokeFree { get; set; }
    public decimal MoneySaved { get; set; }
    public int CigarettesNotSmoked { get; set; }
    public int TotalAchievements { get; set; }
    public int TotalPoints { get; set; }
    public List<ProgressSummary> RecentProgress { get; set; } = new();
    public List<AchievementSummary> RecentAchievements { get; set; } = new();
    public List<NotificationSummary> RecentNotifications { get; set; } = new();
}

public class ProgressSummary
{
    public DateTime Date { get; set; }
    public int CigarettesSmoked { get; set; }
    public decimal MoneySaved { get; set; }
    public string MoodLevel { get; set; } = string.Empty;
    public string EnergyLevel { get; set; } = string.Empty;
}

public class AchievementSummary
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string BadgeLevel { get; set; } = string.Empty;
    public int PointsAwarded { get; set; }
    public DateTime AchievedDate { get; set; }
}

public class NotificationSummary
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public bool IsRead { get; set; }
}

public class StatisticsData
{
    public List<ChartDataPoint> SmokingTrend { get; set; } = new();
    public List<ChartDataPoint> MoneySavedTrend { get; set; } = new();
    public List<ChartDataPoint> MoodTrend { get; set; } = new();
    public List<ChartDataPoint> WeightTrend { get; set; } = new();
}

public class ChartDataPoint
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public string Label { get; set; } = string.Empty;
}

public class MilestoneProgress
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime TargetDate { get; set; }
    public bool IsAchieved { get; set; }
    public int DaysRemaining { get; set; }
    public double ProgressPercentage { get; set; }
    public string? Reward { get; set; }
}