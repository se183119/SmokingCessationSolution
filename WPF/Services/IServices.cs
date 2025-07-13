using BusinessObjects;

namespace WPF.Services;

public interface IAuthenticationService
{
    Task<User?> AuthenticateAsync(string username, string password);
    Task<User?> GetCurrentUserAsync();
    Task SetCurrentUserAsync(User user);
    Task LogoutAsync();
    bool IsAuthenticated { get; }
    User? CurrentUser { get; }
}

public interface INavigationService
{
    void NavigateTo(string viewName);
    void NavigateTo(string viewName, object? parameter);
    void GoBack();
    bool CanGoBack { get; }
}

public interface IUserService
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User> CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role);
    Task<bool> UsernameExistsAsync(string username);
    Task<bool> EmailExistsAsync(string email);
}

public interface ISmokingStatusService
{
    Task<SmokingStatus?> GetLatestStatusAsync(int userId);
    Task<SmokingStatus> CreateStatusAsync(SmokingStatus status);
    Task UpdateStatusAsync(SmokingStatus status);
    Task<IEnumerable<SmokingStatus>> GetUserHistoryAsync(int userId);
    Task<decimal> CalculateMoneySavedAsync(int userId);
}

public interface IQuitPlanService
{
    Task<QuitPlan?> GetActiveQuitPlanAsync(int userId);
    Task<QuitPlan> CreateQuitPlanAsync(QuitPlan plan);
    Task UpdateQuitPlanAsync(QuitPlan plan);
    Task<IEnumerable<QuitPlan>> GetUserPlansAsync(int userId);
    Task<QuitPlan?> GetPlanWithMilestonesAsync(int planId);
    Task UpdateMilestoneAsync(QuitPlanMilestone milestone);
}

public interface IProgressService
{
    Task<ProgressTracking> LogProgressAsync(ProgressTracking progress);
    Task<IEnumerable<ProgressTracking>> GetUserProgressAsync(int userId, DateTime fromDate, DateTime toDate);
    Task<ProgressTracking?> GetTodayProgressAsync(int userId);
    Task<WPF.Models.DashboardData> GetDashboardDataAsync(int userId);
}

public interface IAchievementService
{
    Task<IEnumerable<Achievement>> GetUserAchievementsAsync(int userId);
    Task<Achievement> AwardAchievementAsync(int userId, string title, string description, string category, int points);
    Task CheckAndAwardAchievementsAsync(int userId);
    Task<int> GetUserTotalPointsAsync(int userId);
}

public interface INotificationService
{
    Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId);
    Task<Notification> CreateNotificationAsync(Notification notification);
    Task MarkAsReadAsync(int notificationId);
    Task<int> GetUnreadCountAsync(int userId);
    Task SendMotivationalMessageAsync(int userId);
}

public interface IMembershipService
{
    Task<IEnumerable<MembershipPackage>> GetActivePackagesAsync();
    Task<MembershipSubscription?> GetUserSubscriptionAsync(int userId);
    Task<bool> IsUserSubscribedAsync(int userId);
    Task<MembershipSubscription> SubscribeUserAsync(int userId, int packageId);
}

public interface IConsultationService
{
    Task<Consultation> CreateConsultationAsync(Consultation consultation);
    Task<IEnumerable<Consultation>> GetMemberConsultationsAsync(int memberId);
    Task<IEnumerable<Consultation>> GetCoachConsultationsAsync(int coachId);
    Task<IEnumerable<Consultation>> GetPendingConsultationsAsync();
    Task RespondToConsultationAsync(int consultationId, string response, int coachId);
    Task AssignToCoachAsync(int consultationId, int coachId);
}

public interface IFeedbackService
{
    Task<Feedback> CreateFeedbackAsync(Feedback feedback);
    Task<IEnumerable<Feedback>> GetPublicFeedbackAsync();
    Task<IEnumerable<Feedback>> GetFeedbackByStatusAsync(string status);
    Task<double> GetAverageRatingAsync();
    Task UpdateFeedbackStatusAsync(int feedbackId, string status, string? adminResponse = null);
}