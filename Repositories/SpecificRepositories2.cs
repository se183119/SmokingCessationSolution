using BusinessObjects;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class NotificationRepository : GenericRepository<Notification>, INotificationRepository
{
    public NotificationRepository(SmokingWPFContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
    {
        var query = _dbSet.Where(n => n.UserId == userId && n.IsActive);
        
        if (unreadOnly)
        {
            query = query.Where(n => !n.IsRead);
        }
        
        return await query
            .OrderByDescending(n => n.CreatedDate)
            .ToListAsync();
    }

    public async Task MarkAsReadAsync(int notificationId)
    {
        var notification = await _dbSet.FindAsync(notificationId);
        if (notification != null)
        {
            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MarkAllAsReadAsync(int userId)
    {
        var notifications = await _dbSet
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();
        
        foreach (var notification in notifications)
        {
            notification.IsRead = true;
            notification.ReadDate = DateTime.UtcNow;
        }
        
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetUnreadCountAsync(int userId)
    {
        return await _dbSet
            .CountAsync(n => n.UserId == userId && !n.IsRead && n.IsActive);
    }
}

public class MembershipRepository : GenericRepository<MembershipPackage>, IMembershipRepository
{
    public MembershipRepository(SmokingWPFContext context) : base(context)
    {
    }

    public async Task<IEnumerable<MembershipPackage>> GetActivePackagesAsync()
    {
        return await _dbSet
            .Where(m => m.IsActive)
            .OrderBy(m => m.SortOrder)
            .ToListAsync();
    }

    public async Task<MembershipSubscription?> GetUserSubscriptionAsync(int userId)
    {
        return await _context.MembershipSubscriptions
            .Include(m => m.MembershipPackage)
            .Where(m => m.UserId == userId && m.Status == "Active")
            .FirstOrDefaultAsync();
    }

    public async Task<bool> IsUserSubscribedAsync(int userId)
    {
        return await _context.MembershipSubscriptions
            .AnyAsync(m => m.UserId == userId && m.Status == "Active" && m.EndDate > DateTime.UtcNow);
    }
}

public class ConsultationRepository : GenericRepository<Consultation>, IConsultationRepository
{
    public ConsultationRepository(SmokingWPFContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Consultation>> GetMemberConsultationsAsync(int memberId)
    {
        return await _dbSet
            .Include(c => c.Coach)
            .Where(c => c.MemberId == memberId)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Consultation>> GetCoachConsultationsAsync(int coachId)
    {
        return await _dbSet
            .Include(c => c.Member)
            .Where(c => c.CoachId == coachId)
            .OrderByDescending(c => c.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Consultation>> GetPendingConsultationsAsync()
    {
        return await _dbSet
            .Include(c => c.Member)
            .Where(c => c.Status == "Pending")
            .OrderByDescending(c => c.Priority)
            .ThenBy(c => c.CreatedDate)
            .ToListAsync();
    }

    public async Task AssignToCoachAsync(int consultationId, int coachId)
    {
        var consultation = await _dbSet.FindAsync(consultationId);
        if (consultation != null)
        {
            consultation.CoachId = coachId;
            consultation.Status = "Assigned";
            await _context.SaveChangesAsync();
        }
    }
}

public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
{
    public FeedbackRepository(SmokingWPFContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Feedback>> GetByStatusAsync(string status)
    {
        return await _dbSet
            .Include(f => f.User)
            .Where(f => f.Status == status)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Feedback>> GetPublicFeedbackAsync()
    {
        return await _dbSet
            .Include(f => f.User)
            .Where(f => f.IsPublic && f.Status == "Reviewed")
            .OrderByDescending(f => f.Rating)
            .ThenByDescending(f => f.CreatedDate)
            .ToListAsync();
    }

    public async Task<double> GetAverageRatingAsync()
    {
        var ratings = await _dbSet
            .Where(f => f.Status == "Reviewed")
            .Select(f => f.Rating)
            .ToListAsync();
        
        return ratings.Any() ? ratings.Average() : 0;
    }

    public async Task<IEnumerable<Feedback>> GetByCategoryAsync(string category)
    {
        return await _dbSet
            .Include(f => f.User)
            .Where(f => f.Category == category)
            .OrderByDescending(f => f.CreatedDate)
            .ToListAsync();
    }
}