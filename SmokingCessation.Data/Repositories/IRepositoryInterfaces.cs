using SmokingCessation.Data.Entities;

namespace SmokingCessation.Data.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<IEnumerable<User>> GetUsersByRoleAsync(Common.Enums.UserRole role);
        Task<User?> GetUserWithQuitPlansAsync(int userId);
        Task<User?> GetUserWithSmokeLogsAsync(int userId);
        Task<User?> GetUserWithAchievementsAsync(int userId);
    }

    public interface IQuitPlanRepository : IGenericRepository<QuitPlan>
    {
        Task<IEnumerable<QuitPlan>> GetUserQuitPlansAsync(int userId);
        Task<QuitPlan?> GetActiveQuitPlanAsync(int userId);
        Task<QuitPlan?> GetQuitPlanWithMilestonesAsync(int quitPlanId);
        Task<IEnumerable<QuitPlan>> GetQuitPlansByStatusAsync(Common.Enums.QuitPlanStatus status);
    }

    public interface ISmokeLogRepository : IGenericRepository<SmokeLog>
    {
        Task<IEnumerable<SmokeLog>> GetUserSmokeLogsAsync(int userId);
        Task<IEnumerable<SmokeLog>> GetUserSmokeLogsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<int> GetDaysSmokeFreeAsync(int userId);
        Task<decimal> GetMoneySavedAsync(int userId, decimal costPerPack, int cigarettesPerPack);
        Task<SmokeLog?> GetLastSmokeLogAsync(int userId);
        Task<IEnumerable<SmokeLog>> GetTodaySmokeLogsAsync(int userId);
    }

    public interface IAchievementRepository : IGenericRepository<Achievement>
    {
        Task<IEnumerable<Achievement>> GetActiveAchievementsAsync();
        Task<IEnumerable<Achievement>> GetAchievementsByTypeAsync(Common.Enums.AchievementType type);
    }

    public interface IUserAchievementRepository : IGenericRepository<UserAchievement>
    {
        Task<IEnumerable<UserAchievement>> GetUserAchievementsAsync(int userId);
        Task<IEnumerable<UserAchievement>> GetUnnotifiedAchievementsAsync(int userId);
        Task<bool> HasUserEarnedAchievementAsync(int userId, int achievementId);
    }

    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<IEnumerable<Message>> GetUserMessagesAsync(int userId);
        Task<IEnumerable<Message>> GetConversationAsync(int senderId, int receiverId);
        Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId);
        Task<int> GetUnreadMessageCountAsync(int userId);
        Task MarkAsReadAsync(int messageId);
    }

    public interface ICoachRepository : IGenericRepository<Coach>
    {
        Task<Coach?> GetCoachByUserIdAsync(int userId);
        Task<IEnumerable<Coach>> GetCoachesByStatusAsync(Common.Enums.CoachStatus status);
        Task<IEnumerable<Coach>> GetAvailableCoachesAsync();
        Task<Coach?> GetCoachWithMembersAsync(int coachId);
    }

    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<IEnumerable<Feedback>> GetUserFeedbackAsync(int userId);
        Task<IEnumerable<Feedback>> GetFeedbackByStatusAsync(Common.Enums.FeedbackStatus status);
        Task<IEnumerable<Feedback>> GetPendingFeedbackAsync();
    }

    public interface ITemplateRepository : IGenericRepository<Template>
    {
        Task<IEnumerable<Template>> GetActiveTemplatesAsync();
        Task<IEnumerable<Template>> GetTemplatesByTypeAsync(Common.Enums.TemplateType type);
    }
}