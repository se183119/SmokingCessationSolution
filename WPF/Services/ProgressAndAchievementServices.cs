using BusinessObjects;
using Repositories;
using WPF.Models;

namespace WPF.Services;

public class ProgressService : IProgressService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProgressService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ProgressTracking> LogProgressAsync(ProgressTracking progress)
    {
        var created = await _unitOfWork.ProgressTrackings.AddAsync(progress);
        await _unitOfWork.SaveChangesAsync();
        return created;
    }

    public async Task<IEnumerable<ProgressTracking>> GetUserProgressAsync(int userId, DateTime fromDate, DateTime toDate)
    {
        return await _unitOfWork.ProgressTrackings.GetUserProgressAsync(userId, fromDate, toDate);
    }

    public async Task<ProgressTracking?> GetTodayProgressAsync(int userId)
    {
        return await _unitOfWork.ProgressTrackings.GetByUserAndDateAsync(userId, DateTime.Today);
    }

    public async Task<DashboardData> GetDashboardDataAsync(int userId)
    {
        var dashboardData = new DashboardData();

        // Calculate days smoke-free (simplified calculation)
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user != null)
        {
            dashboardData.DaysSmokeFree = (DateTime.UtcNow - user.CreatedDate).Days;
        }

        // Get money saved
        dashboardData.MoneySaved = await _unitOfWork.SmokingStatuses.GetTotalMoneySavedAsync(userId);

        // Get recent progress
        var recentProgress = await _unitOfWork.ProgressTrackings.GetRecentProgressAsync(userId, 7);
        dashboardData.RecentProgress = recentProgress.Select(p => new ProgressSummary
        {
            Date = p.RecordDate,
            CigarettesSmoked = p.CigarettesSmoked,
            MoneySaved = p.MoneySaved,
            MoodLevel = p.MoodLevel,
            EnergyLevel = p.EnergyLevel
        }).ToList();

        // Get recent achievements
        var achievements = await _unitOfWork.Achievements.GetUserAchievementsAsync(userId);
        dashboardData.RecentAchievements = achievements.Take(5).Select(a => new AchievementSummary
        {
            Title = a.Title,
            Description = a.Description ?? string.Empty,
            Category = a.Category,
            BadgeLevel = a.BadgeLevel,
            PointsAwarded = a.PointsAwarded,
            AchievedDate = a.AchievedDate
        }).ToList();

        // Get total points
        dashboardData.TotalPoints = await _unitOfWork.Achievements.GetUserTotalPointsAsync(userId);
        dashboardData.TotalAchievements = achievements.Count();

        // Calculate cigarettes not smoked (simplified)
        var smokingStatus = await _unitOfWork.SmokingStatuses.GetLatestByUserAsync(userId);
        if (smokingStatus != null)
        {
            dashboardData.CigarettesNotSmoked = smokingStatus.CigarettesPerDay * dashboardData.DaysSmokeFree;
        }

        return dashboardData;
    }
}

public class AchievementService : IAchievementService
{
    private readonly IUnitOfWork _unitOfWork;

    public AchievementService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Achievement>> GetUserAchievementsAsync(int userId)
    {
        return await _unitOfWork.Achievements.GetUserAchievementsAsync(userId);
    }

    public async Task<Achievement> AwardAchievementAsync(int userId, string title, string description, string category, int points)
    {
        var achievement = new Achievement
        {
            UserId = userId,
            Title = title,
            Description = description,
            Category = category,
            PointsAwarded = points,
            AchievedDate = DateTime.UtcNow,
            BadgeLevel = GetBadgeLevel(points)
        };

        var created = await _unitOfWork.Achievements.AddAsync(achievement);
        await _unitOfWork.SaveChangesAsync();
        return created;
    }

    public async Task CheckAndAwardAchievementsAsync(int userId)
    {
        // Check for various achievement conditions
        await CheckDaysSmokeFreeAchievements(userId);
        await CheckMoneySavedAchievements(userId);
        await CheckProgressTrackingAchievements(userId);
    }

    public async Task<int> GetUserTotalPointsAsync(int userId)
    {
        return await _unitOfWork.Achievements.GetUserTotalPointsAsync(userId);
    }

    private async Task CheckDaysSmokeFreeAchievements(int userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null) return;

        var daysSinceJoined = (DateTime.UtcNow - user.CreatedDate).Days;
        var milestones = new[] { 1, 7, 30, 90, 365 };

        foreach (var milestone in milestones)
        {
            if (daysSinceJoined >= milestone)
            {
                var existingAchievement = await _unitOfWork.Achievements
                    .ExistsAsync(a => a.UserId == userId && 
                                     a.Category == "Days Smoke-Free" && 
                                     a.Title.Contains(milestone.ToString()));

                if (!existingAchievement)
                {
                    await AwardAchievementAsync(userId, 
                        $"{milestone} Days Smoke-Free",
                        $"Congratulations! You've been smoke-free for {milestone} days!",
                        "Days Smoke-Free",
                        milestone * 10);
                }
            }
        }
    }

    private async Task CheckMoneySavedAchievements(int userId)
    {
        var moneySaved = await _unitOfWork.SmokingStatuses.GetTotalMoneySavedAsync(userId);
        var milestones = new[] { 50m, 100m, 500m, 1000m, 5000m };

        foreach (var milestone in milestones)
        {
            if (moneySaved >= milestone)
            {
                var existingAchievement = await _unitOfWork.Achievements
                    .ExistsAsync(a => a.UserId == userId && 
                                     a.Category == "Money Saved" && 
                                     a.Title.Contains(milestone.ToString()));

                if (!existingAchievement)
                {
                    await AwardAchievementAsync(userId,
                        $"${milestone} Saved",
                        $"Amazing! You've saved ${milestone} by not smoking!",
                        "Money Saved",
                        (int)(milestone / 10));
                }
            }
        }
    }

    private async Task CheckProgressTrackingAchievements(int userId)
    {
        var recentProgress = await _unitOfWork.ProgressTrackings.GetRecentProgressAsync(userId, 30);
        var consistentDays = recentProgress.Count();

        if (consistentDays >= 7)
        {
            var existingAchievement = await _unitOfWork.Achievements
                .ExistsAsync(a => a.UserId == userId && 
                             a.Category == "Progress Tracking" && 
                             a.Title.Contains("Consistent Tracker"));

            if (!existingAchievement)
            {
                await AwardAchievementAsync(userId,
                    "Consistent Tracker",
                    "Great job! You've been tracking your progress consistently!",
                    "Progress Tracking",
                    50);
            }
        }
    }

    private static string GetBadgeLevel(int points)
    {
        return points switch
        {
            < 50 => "Bronze",
            < 200 => "Silver",
            < 500 => "Gold",
            _ => "Platinum"
        };
    }
}