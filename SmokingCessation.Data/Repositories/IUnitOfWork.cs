namespace SmokingCessation.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IQuitPlanRepository QuitPlans { get; }
        ISmokeLogRepository SmokeLogs { get; }
        IAchievementRepository Achievements { get; }
        IUserAchievementRepository UserAchievements { get; }
        IMessageRepository Messages { get; }
        ICoachRepository Coaches { get; }
        IFeedbackRepository Feedbacks { get; }
        ITemplateRepository Templates { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}