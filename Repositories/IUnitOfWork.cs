namespace Repositories;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ISmokingStatusRepository SmokingStatuses { get; }
    IQuitPlanRepository QuitPlans { get; }
    IProgressTrackingRepository ProgressTrackings { get; }
    IAchievementRepository Achievements { get; }
    INotificationRepository Notifications { get; }
    IMembershipRepository Memberships { get; }
    IConsultationRepository Consultations { get; }
    IFeedbackRepository Feedbacks { get; }
    
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}