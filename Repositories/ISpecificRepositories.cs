using BusinessObjects;

namespace Repositories;

// Specific repository interfaces
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ValidatePasswordAsync(string username, string password);
    Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
    Task<User?> AuthenticateAsync(string username, string password);
}

public interface ISmokingStatusRepository : IGenericRepository<SmokingStatus>
{
    Task<SmokingStatus?> GetLatestByUserAsync(int userId);
    Task<IEnumerable<SmokingStatus>> GetUserHistoryAsync(int userId);
    Task<decimal> GetTotalMoneySavedAsync(int userId);
}

public interface IQuitPlanRepository : IGenericRepository<QuitPlan>
{
    Task<QuitPlan?> GetActiveByUserAsync(int userId);
    Task<IEnumerable<QuitPlan>> GetUserPlansAsync(int userId);
    Task<QuitPlan?> GetWithMilestonesAsync(int planId);
}

public interface IProgressTrackingRepository : IGenericRepository<ProgressTracking>
{
    Task<IEnumerable<ProgressTracking>> GetUserProgressAsync(int userId, DateTime fromDate, DateTime toDate);
    Task<ProgressTracking?> GetByUserAndDateAsync(int userId, DateTime date);
    Task<IEnumerable<ProgressTracking>> GetRecentProgressAsync(int userId, int days);
}

public interface IAchievementRepository : IGenericRepository<Achievement>
{
    Task<IEnumerable<Achievement>> GetUserAchievementsAsync(int userId);
    Task<IEnumerable<Achievement>> GetByUserAndCategoryAsync(int userId, string category);
    Task<int> GetUserTotalPointsAsync(int userId);
}

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);
    Task MarkAsReadAsync(int notificationId);
    Task MarkAllAsReadAsync(int userId);
    Task<int> GetUnreadCountAsync(int userId);
}

public interface IMembershipRepository : IGenericRepository<MembershipPackage>
{
    Task<IEnumerable<MembershipPackage>> GetActivePackagesAsync();
    Task<MembershipSubscription?> GetUserSubscriptionAsync(int userId);
    Task<bool> IsUserSubscribedAsync(int userId);
}

public interface IConsultationRepository : IGenericRepository<Consultation>
{
    Task<IEnumerable<Consultation>> GetMemberConsultationsAsync(int memberId);
    Task<IEnumerable<Consultation>> GetCoachConsultationsAsync(int coachId);
    Task<IEnumerable<Consultation>> GetPendingConsultationsAsync();
    Task AssignToCoachAsync(int consultationId, int coachId);
}

public interface IFeedbackRepository : IGenericRepository<Feedback>
{
    Task<IEnumerable<Feedback>> GetByStatusAsync(string status);
    Task<IEnumerable<Feedback>> GetPublicFeedbackAsync();
    Task<double> GetAverageRatingAsync();
    Task<IEnumerable<Feedback>> GetByCategoryAsync(string category);
}