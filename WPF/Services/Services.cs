using BusinessObjects;
using Repositories;

namespace WPF.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUnitOfWork _unitOfWork;
    private User? _currentUser;

    public AuthenticationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public User? CurrentUser => _currentUser;
    public bool IsAuthenticated => _currentUser != null;

    public async Task<User?> AuthenticateAsync(string username, string password)
    {
        try
        {
            var user = await _unitOfWork.Users.AuthenticateAsync(username, password);
            return user;
        }
        catch
        {
            return null;
        }
    }

    public async Task<User?> GetCurrentUserAsync()
    {
        return await Task.FromResult(_currentUser);
    }

    public async Task SetCurrentUserAsync(User user)
    {
        _currentUser = user;
        await Task.CompletedTask;
    }

    public async Task LogoutAsync()
    {
        _currentUser = null;
        await Task.CompletedTask;
    }
}

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _unitOfWork.Users.GetByIdAsync(id);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        // Hash password before saving
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        
        var createdUser = await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();
        return createdUser;
    }

    public async Task UpdateUserAsync(User user)
    {
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<User>> GetUsersByRoleAsync(UserRole role)
    {
        return await _unitOfWork.Users.GetUsersByRoleAsync(role);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _unitOfWork.Users.ExistsAsync(u => u.Username == username);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _unitOfWork.Users.ExistsAsync(u => u.Email == email);
    }
}

public class SmokingStatusService : ISmokingStatusService
{
    private readonly IUnitOfWork _unitOfWork;

    public SmokingStatusService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<SmokingStatus?> GetLatestStatusAsync(int userId)
    {
        return await _unitOfWork.SmokingStatuses.GetLatestByUserAsync(userId);
    }

    public async Task<SmokingStatus> CreateStatusAsync(SmokingStatus status)
    {
        var created = await _unitOfWork.SmokingStatuses.AddAsync(status);
        await _unitOfWork.SaveChangesAsync();
        return created;
    }

    public async Task UpdateStatusAsync(SmokingStatus status)
    {
        await _unitOfWork.SmokingStatuses.UpdateAsync(status);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<SmokingStatus>> GetUserHistoryAsync(int userId)
    {
        return await _unitOfWork.SmokingStatuses.GetUserHistoryAsync(userId);
    }

    public async Task<decimal> CalculateMoneySavedAsync(int userId)
    {
        return await _unitOfWork.SmokingStatuses.GetTotalMoneySavedAsync(userId);
    }
}

public class QuitPlanService : IQuitPlanService
{
    private readonly IUnitOfWork _unitOfWork;

    public QuitPlanService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<QuitPlan?> GetActiveQuitPlanAsync(int userId)
    {
        return await _unitOfWork.QuitPlans.GetActiveByUserAsync(userId);
    }

    public async Task<QuitPlan> CreateQuitPlanAsync(QuitPlan plan)
    {
        var created = await _unitOfWork.QuitPlans.AddAsync(plan);
        await _unitOfWork.SaveChangesAsync();
        return created;
    }

    public async Task UpdateQuitPlanAsync(QuitPlan plan)
    {
        await _unitOfWork.QuitPlans.UpdateAsync(plan);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<QuitPlan>> GetUserPlansAsync(int userId)
    {
        return await _unitOfWork.QuitPlans.GetUserPlansAsync(userId);
    }

    public async Task<QuitPlan?> GetPlanWithMilestonesAsync(int planId)
    {
        return await _unitOfWork.QuitPlans.GetWithMilestonesAsync(planId);
    }

    public async Task UpdateMilestoneAsync(QuitPlanMilestone milestone)
    {
        // This would need a specific repository method
        await Task.CompletedTask;
    }
}