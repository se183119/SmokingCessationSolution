using BusinessObjects;
using WPF.Services;
using WPF.Models;

namespace WPF.ViewModels;

public class DashboardViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly IProgressService _progressService;
    private readonly IAuthenticationService _authService;
    private User? _currentUser;
    private DashboardData? _dashboardData;
    private bool _isLoading = true;

    public DashboardViewModel(
        IUserService userService, 
        IProgressService progressService,
        IAuthenticationService authService)
    {
        _userService = userService;
        _progressService = progressService;
        _authService = authService;
        RefreshCommand = new RelayCommand(async () => await RefreshDataAsync());
        LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
    }

    public User? CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    public DashboardData? DashboardData
    {
        get => _dashboardData;
        set => SetProperty(ref _dashboardData, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand RefreshCommand { get; }
    public ICommand LoadDataCommand { get; }

    public async Task InitializeAsync()
    {
        CurrentUser = await _authService.GetCurrentUserAsync();
        if (CurrentUser != null)
        {
            await LoadDataAsync();
        }
    }

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            
            if (CurrentUser == null) return;

            var data = await _progressService.GetDashboardDataAsync(CurrentUser.Id);
            DashboardData = data;
        }
        catch (Exception ex)
        {
            // Log error - in a real application, you'd use a logging service
            Console.WriteLine($"Error loading dashboard data: {ex.Message}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task RefreshDataAsync()
    {
        await LoadDataAsync();
    }
}

public class MemberDashboardViewModel : DashboardViewModel
{
    private readonly ISmokingStatusService _smokingStatusService;
    private readonly IQuitPlanService _quitPlanService;
    private readonly IAchievementService _achievementService;

    public MemberDashboardViewModel(
        IUserService userService,
        IProgressService progressService,
        IAuthenticationService authService,
        ISmokingStatusService smokingStatusService,
        IQuitPlanService quitPlanService,
        IAchievementService achievementService) 
        : base(userService, progressService, authService)
    {
        _smokingStatusService = smokingStatusService;
        _quitPlanService = quitPlanService;
        _achievementService = achievementService;
        
        ViewProgressCommand = new RelayCommand(NavigateToProgress);
        ViewQuitPlanCommand = new RelayCommand(NavigateToQuitPlan);
        LogTodayCommand = new RelayCommand(async () => await LogTodayAsync());
    }

    public ICommand ViewProgressCommand { get; }
    public ICommand ViewQuitPlanCommand { get; }
    public ICommand LogTodayCommand { get; }

    private void NavigateToProgress()
    {
        // Navigate to progress tracking window
    }

    private void NavigateToQuitPlan()
    {
        // Navigate to quit plan window
    }

    private async Task LogTodayAsync()
    {
        // Navigate to daily logging
        await Task.CompletedTask;
    }
}