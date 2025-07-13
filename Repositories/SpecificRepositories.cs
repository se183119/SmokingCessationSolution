using BusinessObjects;
using DataAccessObjects;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(SmokingWPFContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> ValidatePasswordAsync(string username, string password)
    {
        var user = await GetByUsernameAsync(username);
        if (user == null) return false;
        
        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
    {
        return await _dbSet.Where(u => u.Role == role && u.IsActive).ToListAsync();
    }

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await GetByUsernameAsync(username);
        if (user == null || !user.IsActive) return null;
        
        if (BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            user.LastLoginDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return user;
        }
        
        return null;
    }
}

public class SmokingStatusRepository : GenericRepository<SmokingStatus>, ISmokingStatusRepository
{
    public SmokingStatusRepository(SmokingWPFContext context) : base(context)
    {
    }

    public async Task<SmokingStatus?> GetLatestByUserAsync(int userId)
    {
        return await _dbSet
            .Where(s => s.UserId == userId && s.IsActive)
            .OrderByDescending(s => s.RecordedDate)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<SmokingStatus>> GetUserHistoryAsync(int userId)
    {
        return await _dbSet
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.RecordedDate)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalMoneySavedAsync(int userId)
    {
        var latestStatus = await GetLatestByUserAsync(userId);
        if (latestStatus == null) return 0;

        var user = await _context.Users.FindAsync(userId);
        if (user == null) return 0;

        var daysSinceCreated = (DateTime.UtcNow - user.CreatedDate).Days;
        return latestStatus.DailyCost * daysSinceCreated;
    }
}

public class QuitPlanRepository : GenericRepository<QuitPlan>, IQuitPlanRepository
{
    public QuitPlanRepository(SmokingWPFContext context) : base(context)
    {
    }

    public async Task<QuitPlan?> GetActiveByUserAsync(int userId)
    {
        return await _dbSet
            .Include(q => q.Milestones)
            .Where(q => q.UserId == userId && q.IsActive)
            .OrderByDescending(q => q.CreatedDate)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<QuitPlan>> GetUserPlansAsync(int userId)
    {
        return await _dbSet
            .Include(q => q.Milestones)
            .Where(q => q.UserId == userId)
            .OrderByDescending(q => q.CreatedDate)
            .ToListAsync();
    }

    public async Task<QuitPlan?> GetWithMilestonesAsync(int planId)
    {
        return await _dbSet
            .Include(q => q.Milestones)
            .FirstOrDefaultAsync(q => q.Id == planId);
    }
}

public class ProgressTrackingRepository : GenericRepository<ProgressTracking>, IProgressTrackingRepository
{
    public ProgressTrackingRepository(SmokingWPFContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ProgressTracking>> GetUserProgressAsync(int userId, DateTime fromDate, DateTime toDate)
    {
        return await _dbSet
            .Where(p => p.UserId == userId && p.RecordDate >= fromDate && p.RecordDate <= toDate)
            .OrderBy(p => p.RecordDate)
            .ToListAsync();
    }

    public async Task<ProgressTracking?> GetByUserAndDateAsync(int userId, DateTime date)
    {
        return await _dbSet
            .FirstOrDefaultAsync(p => p.UserId == userId && p.RecordDate.Date == date.Date);
    }

    public async Task<IEnumerable<ProgressTracking>> GetRecentProgressAsync(int userId, int days)
    {
        var fromDate = DateTime.UtcNow.AddDays(-days);
        return await GetUserProgressAsync(userId, fromDate, DateTime.UtcNow);
    }
}

public class AchievementRepository : GenericRepository<Achievement>, IAchievementRepository
{
    public AchievementRepository(SmokingWPFContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Achievement>> GetUserAchievementsAsync(int userId)
    {
        return await _dbSet
            .Where(a => a.UserId == userId && a.IsVisible)
            .OrderByDescending(a => a.AchievedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Achievement>> GetByUserAndCategoryAsync(int userId, string category)
    {
        return await _dbSet
            .Where(a => a.UserId == userId && a.Category == category && a.IsVisible)
            .OrderByDescending(a => a.AchievedDate)
            .ToListAsync();
    }

    public async Task<int> GetUserTotalPointsAsync(int userId)
    {
        return await _dbSet
            .Where(a => a.UserId == userId)
            .SumAsync(a => a.PointsAwarded);
    }
}