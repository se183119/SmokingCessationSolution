using Microsoft.EntityFrameworkCore;
using SmokingCessation.Data.Context;
using SmokingCessation.Data.Entities;
using SmokingCessation.Common.Enums;

namespace SmokingCessation.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SmokingCessationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
        {
            return await _context.Users.Where(u => u.Role == role).ToListAsync();
        }

        public async Task<User?> GetUserWithQuitPlansAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.QuitPlans)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserWithSmokeLogsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.SmokeLogs)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserWithAchievementsAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.UserAchievements)
                    .ThenInclude(ua => ua.Achievement)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }
    }

    public class SmokeLogRepository : GenericRepository<SmokeLog>, ISmokeLogRepository
    {
        public SmokeLogRepository(SmokingCessationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SmokeLog>> GetUserSmokeLogsAsync(int userId)
        {
            return await _context.SmokeLogs
                .Where(sl => sl.UserId == userId)
                .OrderByDescending(sl => sl.LogDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<SmokeLog>> GetUserSmokeLogsByDateRangeAsync(int userId, DateTime startDate, DateTime endDate)
        {
            return await _context.SmokeLogs
                .Where(sl => sl.UserId == userId && sl.LogDate >= startDate && sl.LogDate <= endDate)
                .OrderByDescending(sl => sl.LogDate)
                .ToListAsync();
        }

        public async Task<int> GetDaysSmokeFreeAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user?.SmokingQuitDate == null)
                return 0;

            var lastSmokeLog = await _context.SmokeLogs
                .Where(sl => sl.UserId == userId && sl.LogType == SmokeLogType.Smoked)
                .OrderByDescending(sl => sl.LogDate)
                .FirstOrDefaultAsync();

            var referenceDate = lastSmokeLog?.LogDate ?? user.SmokingQuitDate.Value;
            return (DateTime.Now.Date - referenceDate.Date).Days;
        }

        public async Task<decimal> GetMoneySavedAsync(int userId, decimal costPerPack, int cigarettesPerPack)
        {
            var daysSmokeFree = await GetDaysSmokeFreeAsync(userId);
            var user = await _context.Users.FindAsync(userId);
            
            if (user?.CigarettesPerDay == null || cigarettesPerPack == 0)
                return 0;

            var packsPerDay = (decimal)user.CigarettesPerDay.Value / cigarettesPerPack;
            return daysSmokeFree * packsPerDay * costPerPack;
        }

        public async Task<SmokeLog?> GetLastSmokeLogAsync(int userId)
        {
            return await _context.SmokeLogs
                .Where(sl => sl.UserId == userId)
                .OrderByDescending(sl => sl.LogDate)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SmokeLog>> GetTodaySmokeLogsAsync(int userId)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            
            return await _context.SmokeLogs
                .Where(sl => sl.UserId == userId && sl.LogDate >= today && sl.LogDate < tomorrow)
                .OrderByDescending(sl => sl.LogDate)
                .ToListAsync();
        }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly SmokingCessationDbContext _context;
        private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _transaction;

        public UnitOfWork(SmokingCessationDbContext context)
        {
            _context = context;
            Users = new UserRepository(_context);
            QuitPlans = new QuitPlanRepository(_context);
            SmokeLogs = new SmokeLogRepository(_context);
            Achievements = new AchievementRepository(_context);
            UserAchievements = new UserAchievementRepository(_context);
            Messages = new MessageRepository(_context);
            Coaches = new CoachRepository(_context);
            Feedbacks = new FeedbackRepository(_context);
            Templates = new TemplateRepository(_context);
        }

        public IUserRepository Users { get; }
        public IQuitPlanRepository QuitPlans { get; }
        public ISmokeLogRepository SmokeLogs { get; }
        public IAchievementRepository Achievements { get; }
        public IUserAchievementRepository UserAchievements { get; }
        public IMessageRepository Messages { get; }
        public ICoachRepository Coaches { get; }
        public IFeedbackRepository Feedbacks { get; }
        public ITemplateRepository Templates { get; }

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

    // Additional repository implementations
    public class QuitPlanRepository : GenericRepository<QuitPlan>, IQuitPlanRepository
    {
        public QuitPlanRepository(SmokingCessationDbContext context) : base(context) { }

        public async Task<IEnumerable<QuitPlan>> GetUserQuitPlansAsync(int userId)
        {
            return await _context.QuitPlans
                .Where(qp => qp.UserId == userId)
                .Include(qp => qp.Milestones)
                .OrderByDescending(qp => qp.CreatedAt)
                .ToListAsync();
        }

        public async Task<QuitPlan?> GetActiveQuitPlanAsync(int userId)
        {
            return await _context.QuitPlans
                .Where(qp => qp.UserId == userId && qp.Status == QuitPlanStatus.Active)
                .Include(qp => qp.Milestones)
                .FirstOrDefaultAsync();
        }

        public async Task<QuitPlan?> GetQuitPlanWithMilestonesAsync(int quitPlanId)
        {
            return await _context.QuitPlans
                .Include(qp => qp.Milestones)
                .FirstOrDefaultAsync(qp => qp.QuitPlanId == quitPlanId);
        }

        public async Task<IEnumerable<QuitPlan>> GetQuitPlansByStatusAsync(QuitPlanStatus status)
        {
            return await _context.QuitPlans
                .Where(qp => qp.Status == status)
                .Include(qp => qp.User)
                .ToListAsync();
        }
    }

    public class AchievementRepository : GenericRepository<Achievement>, IAchievementRepository
    {
        public AchievementRepository(SmokingCessationDbContext context) : base(context) { }

        public async Task<IEnumerable<Achievement>> GetActiveAchievementsAsync()
        {
            return await _context.Achievements
                .Where(a => a.IsActive)
                .OrderBy(a => a.RequiredDays ?? 0)
                .ThenBy(a => a.RequiredAmount ?? 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<Achievement>> GetAchievementsByTypeAsync(AchievementType type)
        {
            return await _context.Achievements
                .Where(a => a.Type == type && a.IsActive)
                .OrderBy(a => a.RequiredDays ?? 0)
                .ThenBy(a => a.RequiredAmount ?? 0)
                .ToListAsync();
        }
    }

    public class UserAchievementRepository : GenericRepository<UserAchievement>, IUserAchievementRepository
    {
        public UserAchievementRepository(SmokingCessationDbContext context) : base(context) { }

        public async Task<IEnumerable<UserAchievement>> GetUserAchievementsAsync(int userId)
        {
            return await _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .Include(ua => ua.Achievement)
                .OrderByDescending(ua => ua.EarnedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserAchievement>> GetUnnotifiedAchievementsAsync(int userId)
        {
            return await _context.UserAchievements
                .Where(ua => ua.UserId == userId && !ua.IsNotified)
                .Include(ua => ua.Achievement)
                .ToListAsync();
        }

        public async Task<bool> HasUserEarnedAchievementAsync(int userId, int achievementId)
        {
            return await _context.UserAchievements
                .AnyAsync(ua => ua.UserId == userId && ua.AchievementId == achievementId);
        }
    }

    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(SmokingCessationDbContext context) : base(context) { }

        public async Task<IEnumerable<Message>> GetUserMessagesAsync(int userId)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userId || m.ReceiverId == userId) && !m.IsDeleted)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetConversationAsync(int senderId, int receiverId)
        {
            return await _context.Messages
                .Where(m => ((m.SenderId == senderId && m.ReceiverId == receiverId) ||
                           (m.SenderId == receiverId && m.ReceiverId == senderId)) && !m.IsDeleted)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetUnreadMessagesAsync(int userId)
        {
            return await _context.Messages
                .Where(m => m.ReceiverId == userId && m.ReadAt == null && !m.IsDeleted)
                .Include(m => m.Sender)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<int> GetUnreadMessageCountAsync(int userId)
        {
            return await _context.Messages
                .CountAsync(m => m.ReceiverId == userId && m.ReadAt == null && !m.IsDeleted);
        }

        public async Task MarkAsReadAsync(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            if (message != null && message.ReadAt == null)
            {
                message.ReadAt = DateTime.UtcNow;
                message.Status = MessageStatus.Read;
                await _context.SaveChangesAsync();
            }
        }
    }

    public class CoachRepository : GenericRepository<Coach>, ICoachRepository
    {
        public CoachRepository(SmokingCessationDbContext context) : base(context) { }

        public async Task<Coach?> GetCoachByUserIdAsync(int userId)
        {
            return await _context.Coaches
                .Include(c => c.User)
                .Include(c => c.AssignedMembers)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<IEnumerable<Coach>> GetCoachesByStatusAsync(CoachStatus status)
        {
            return await _context.Coaches
                .Where(c => c.Status == status)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Coach>> GetAvailableCoachesAsync()
        {
            return await _context.Coaches
                .Where(c => c.Status == CoachStatus.Approved && c.IsAvailable)
                .Include(c => c.User)
                .Include(c => c.AssignedMembers)
                .Where(c => c.AssignedMembers.Count < c.MaxMembers)
                .ToListAsync();
        }

        public async Task<Coach?> GetCoachWithMembersAsync(int coachId)
        {
            return await _context.Coaches
                .Include(c => c.User)
                .Include(c => c.AssignedMembers)
                .FirstOrDefaultAsync(c => c.CoachId == coachId);
        }
    }

    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(SmokingCessationDbContext context) : base(context) { }

        public async Task<IEnumerable<Feedback>> GetUserFeedbackAsync(int userId)
        {
            return await _context.Feedbacks
                .Where(f => f.UserId == userId)
                .Include(f => f.ResolvedByUser)
                .OrderByDescending(f => f.SubmittedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetFeedbackByStatusAsync(FeedbackStatus status)
        {
            return await _context.Feedbacks
                .Where(f => f.Status == status)
                .Include(f => f.User)
                .Include(f => f.ResolvedByUser)
                .OrderBy(f => f.SubmittedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Feedback>> GetPendingFeedbackAsync()
        {
            return await _context.Feedbacks
                .Where(f => f.Status == FeedbackStatus.Submitted || f.Status == FeedbackStatus.InReview)
                .Include(f => f.User)
                .OrderBy(f => f.SubmittedAt)
                .ToListAsync();
        }
    }

    public class TemplateRepository : GenericRepository<Template>, ITemplateRepository
    {
        public TemplateRepository(SmokingCessationDbContext context) : base(context) { }

        public async Task<IEnumerable<Template>> GetActiveTemplatesAsync()
        {
            return await _context.Templates
                .Where(t => t.IsActive)
                .Include(t => t.CreatedByUser)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Template>> GetTemplatesByTypeAsync(TemplateType type)
        {
            return await _context.Templates
                .Where(t => t.Type == type && t.IsActive)
                .Include(t => t.CreatedByUser)
                .OrderBy(t => t.Name)
                .ToListAsync();
        }
    }
}