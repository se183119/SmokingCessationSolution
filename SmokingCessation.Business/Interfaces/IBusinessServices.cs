using SmokingCessation.Business.DTOs;

namespace SmokingCessation.Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<(bool Success, string? Token, UserDto? User)> LoginAsync(LoginDto loginDto);
        Task<(bool Success, string? ErrorMessage)> RegisterAsync(RegisterDto registerDto);
        Task LogoutAsync(int userId);
        Task<(bool Success, string? ErrorMessage)> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);
        Task<bool> ValidateTokenAsync(string token);
        Task<UserDto?> GetCurrentUserAsync(string token);
        Task<(bool Success, string? ErrorMessage)> ResetPasswordAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
    }

    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<IEnumerable<UserDto>> GetUsersByRoleAsync(Common.Enums.UserRole role);
        Task<(bool Success, string? ErrorMessage)> UpdateProfileAsync(int userId, UpdateProfileDto updateProfileDto);
        Task<DashboardStatsDto> GetDashboardStatsAsync(int userId);
        Task<bool> DeactivateUserAsync(int userId);
        Task<bool> ActivateUserAsync(int userId);
        Task<IEnumerable<UserDto>> SearchUsersAsync(string searchTerm);
        Task<UserDto?> GetUserWithDetailsAsync(int userId);
    }

    public interface IQuitPlanService
    {
        Task<IEnumerable<QuitPlanDto>> GetUserQuitPlansAsync(int userId);
        Task<QuitPlanDto?> GetQuitPlanByIdAsync(int quitPlanId);
        Task<QuitPlanDto?> GetActiveQuitPlanAsync(int userId);
        Task<(bool Success, int? QuitPlanId, string? ErrorMessage)> CreateQuitPlanAsync(int userId, CreateQuitPlanDto createQuitPlanDto);
        Task<(bool Success, string? ErrorMessage)> UpdateQuitPlanAsync(int quitPlanId, CreateQuitPlanDto updateQuitPlanDto);
        Task<(bool Success, string? ErrorMessage)> DeleteQuitPlanAsync(int quitPlanId);
        Task<(bool Success, string? ErrorMessage)> ActivateQuitPlanAsync(int quitPlanId);
        Task<(bool Success, string? ErrorMessage)> CompleteQuitPlanAsync(int quitPlanId);
        Task<(bool Success, string? ErrorMessage)> CompleteMilestoneAsync(int milestoneId);
        Task<QuitPlanDto?> GetQuitPlanWithMilestonesAsync(int quitPlanId);
    }

    public interface ISmokeLogService
    {
        Task<IEnumerable<SmokeLogDto>> GetUserSmokeLogsAsync(int userId);
        Task<IEnumerable<SmokeLogDto>> GetUserSmokeLogsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate);
        Task<SmokeLogDto?> GetSmokeLogByIdAsync(int smokeLogId);
        Task<(bool Success, int? SmokeLogId, string? ErrorMessage)> CreateSmokeLogAsync(int userId, CreateSmokeLogDto createSmokeLogDto);
        Task<(bool Success, string? ErrorMessage)> UpdateSmokeLogAsync(int smokeLogId, CreateSmokeLogDto updateSmokeLogDto);
        Task<(bool Success, string? ErrorMessage)> DeleteSmokeLogAsync(int smokeLogId);
        Task<DashboardStatsDto> GetUserStatsAsync(int userId);
        Task<ProgressChartDto> GetProgressChartDataAsync(int userId, int days = 30);
        Task<IEnumerable<SmokeLogDto>> GetTodaySmokeLogsAsync(int userId);
        Task<int> GetStreakAsync(int userId);
    }

    public interface IAchievementService
    {
        Task<IEnumerable<AchievementDto>> GetUserAchievementsAsync(int userId);
        Task<IEnumerable<AchievementDto>> GetAvailableAchievementsAsync(int userId);
        Task<IEnumerable<AchievementDto>> GetUnnotifiedAchievementsAsync(int userId);
        Task MarkAchievementsAsNotifiedAsync(int userId, IEnumerable<int> achievementIds);
        Task CheckAndAwardAchievementsAsync(int userId);
        Task<IEnumerable<AchievementDto>> GetAllAchievementsAsync();
        Task<(bool Success, string? ErrorMessage)> CreateAchievementAsync(AchievementDto achievementDto);
        Task<(bool Success, string? ErrorMessage)> UpdateAchievementAsync(int achievementId, AchievementDto achievementDto);
        Task<(bool Success, string? ErrorMessage)> DeleteAchievementAsync(int achievementId);
    }

    public interface IMessageService
    {
        Task<IEnumerable<MessageDto>> GetUserMessagesAsync(int userId);
        Task<IEnumerable<MessageDto>> GetConversationAsync(int senderId, int receiverId);
        Task<MessageDto?> GetMessageByIdAsync(int messageId);
        Task<(bool Success, int? MessageId, string? ErrorMessage)> SendMessageAsync(int senderId, SendMessageDto sendMessageDto);
        Task<(bool Success, string? ErrorMessage)> MarkAsReadAsync(int messageId, int userId);
        Task<(bool Success, string? ErrorMessage)> DeleteMessageAsync(int messageId, int userId);
        Task<int> GetUnreadMessageCountAsync(int userId);
        Task<IEnumerable<MessageDto>> GetUnreadMessagesAsync(int userId);
    }

    public interface ICoachService
    {
        Task<IEnumerable<CoachDto>> GetAvailableCoachesAsync();
        Task<CoachDto?> GetCoachByUserIdAsync(int userId);
        Task<CoachDto?> GetCoachByIdAsync(int coachId);
        Task<IEnumerable<CoachDto>> GetCoachesByStatusAsync(Common.Enums.CoachStatus status);
        Task<(bool Success, string? ErrorMessage)> ApplyAsCoachAsync(int userId, ApplyAsCoachDto applyAsCoachDto);
        Task<(bool Success, string? ErrorMessage)> ApproveCoachAsync(int coachId, int approvedByUserId);
        Task<(bool Success, string? ErrorMessage)> RejectCoachAsync(int coachId, int rejectedByUserId);
        Task<(bool Success, string? ErrorMessage)> SuspendCoachAsync(int coachId, int suspendedByUserId);
        Task<(bool Success, string? ErrorMessage)> AssignMemberToCoachAsync(int memberId, int coachId);
        Task<(bool Success, string? ErrorMessage)> RemoveMemberFromCoachAsync(int memberId);
        Task<IEnumerable<UserDto>> GetCoachMembersAsync(int coachId);
    }

    public interface IFeedbackService
    {
        Task<IEnumerable<FeedbackDto>> GetUserFeedbackAsync(int userId);
        Task<IEnumerable<FeedbackDto>> GetPendingFeedbackAsync();
        Task<FeedbackDto?> GetFeedbackByIdAsync(int feedbackId);
        Task<(bool Success, int? FeedbackId, string? ErrorMessage)> SubmitFeedbackAsync(int userId, SubmitFeedbackDto submitFeedbackDto);
        Task<(bool Success, string? ErrorMessage)> ResolveFeedbackAsync(int feedbackId, int resolvedByUserId, string response);
        Task<(bool Success, string? ErrorMessage)> CloseFeedbackAsync(int feedbackId);
        Task<IEnumerable<FeedbackDto>> GetFeedbackByStatusAsync(Common.Enums.FeedbackStatus status);
    }
}