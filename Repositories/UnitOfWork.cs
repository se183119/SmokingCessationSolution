using DataAccessObjects;
using Microsoft.EntityFrameworkCore.Storage;

namespace Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly SmokingWPFContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(SmokingWPFContext context)
    {
        _context = context;
        Users = new UserRepository(_context);
        SmokingStatuses = new SmokingStatusRepository(_context);
        QuitPlans = new QuitPlanRepository(_context);
        ProgressTrackings = new ProgressTrackingRepository(_context);
        Achievements = new AchievementRepository(_context);
        Notifications = new NotificationRepository(_context);
        Memberships = new MembershipRepository(_context);
        Consultations = new ConsultationRepository(_context);
        Feedbacks = new FeedbackRepository(_context);
    }

    public IUserRepository Users { get; private set; }
    public ISmokingStatusRepository SmokingStatuses { get; private set; }
    public IQuitPlanRepository QuitPlans { get; private set; }
    public IProgressTrackingRepository ProgressTrackings { get; private set; }
    public IAchievementRepository Achievements { get; private set; }
    public INotificationRepository Notifications { get; private set; }
    public IMembershipRepository Memberships { get; private set; }
    public IConsultationRepository Consultations { get; private set; }
    public IFeedbackRepository Feedbacks { get; private set; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}