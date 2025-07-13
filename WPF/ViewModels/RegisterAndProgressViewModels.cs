using BusinessObjects;
using WPF.Services;

namespace WPF.ViewModels;

public class RegisterViewModel : ViewModelBase
{
    private readonly IUserService _userService;
    private readonly INavigationService _navigationService;
    private readonly IAuthenticationService _authService;

    private string _username = string.Empty;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _confirmPassword = string.Empty;
    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private DateTime? _dateOfBirth;
    private string _gender = string.Empty;
    private string _phoneNumber = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading = false;

    public RegisterViewModel(
        IUserService userService, 
        INavigationService navigationService,
        IAuthenticationService authService)
    {
        _userService = userService;
        _navigationService = navigationService;
        _authService = authService;
        
        RegisterCommand = new RelayCommand(async () => await RegisterAsync(), () => CanRegister());
        BackToLoginCommand = new RelayCommand(() => _navigationService.NavigateTo("Login"));
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set => SetProperty(ref _confirmPassword, value);
    }

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    public DateTime? DateOfBirth
    {
        get => _dateOfBirth;
        set => SetProperty(ref _dateOfBirth, value);
    }

    public string Gender
    {
        get => _gender;
        set => SetProperty(ref _gender, value);
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand RegisterCommand { get; }
    public ICommand BackToLoginCommand { get; }

    private bool CanRegister()
    {
        return !string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Email) &&
               !string.IsNullOrWhiteSpace(Password) &&
               !string.IsNullOrWhiteSpace(ConfirmPassword) &&
               !string.IsNullOrWhiteSpace(FirstName) &&
               !string.IsNullOrWhiteSpace(LastName) &&
               Password == ConfirmPassword &&
               !IsLoading;
    }

    private async Task RegisterAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            // Validate input
            if (Password != ConfirmPassword)
            {
                ErrorMessage = "Passwords do not match.";
                return;
            }

            if (await _userService.UsernameExistsAsync(Username))
            {
                ErrorMessage = "Username already exists.";
                return;
            }

            if (await _userService.EmailExistsAsync(Email))
            {
                ErrorMessage = "Email already exists.";
                return;
            }

            // Create new user
            var user = new User
            {
                Username = Username,
                Email = Email,
                PasswordHash = Password, // Will be hashed in UserService
                FirstName = FirstName,
                LastName = LastName,
                DateOfBirth = DateOfBirth,
                Gender = string.IsNullOrWhiteSpace(Gender) ? null : Gender,
                PhoneNumber = string.IsNullOrWhiteSpace(PhoneNumber) ? null : PhoneNumber,
                Role = UserRole.Member, // Default role for registration
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            };

            var createdUser = await _userService.CreateUserAsync(user);
            
            // Auto-login after successful registration
            await _authService.SetCurrentUserAsync(createdUser);
            _navigationService.NavigateTo("MemberDashboard");
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Registration failed: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}

public class ProgressTrackingViewModel : ViewModelBase
{
    private readonly IProgressService _progressService;
    private readonly IAuthenticationService _authService;
    private readonly IAchievementService _achievementService;

    private ProgressTracking _todayProgress = new();
    private List<ProgressTracking> _recentProgress = new();
    private bool _isLoading = false;
    private string _errorMessage = string.Empty;

    public ProgressTrackingViewModel(
        IProgressService progressService,
        IAuthenticationService authService,
        IAchievementService achievementService)
    {
        _progressService = progressService;
        _authService = authService;
        _achievementService = achievementService;

        SaveProgressCommand = new RelayCommand(async () => await SaveProgressAsync(), () => CanSaveProgress());
        LoadDataCommand = new RelayCommand(async () => await LoadDataAsync());
        
        // Initialize with today's date
        TodayProgress.RecordDate = DateTime.Today;
    }

    public ProgressTracking TodayProgress
    {
        get => _todayProgress;
        set => SetProperty(ref _todayProgress, value);
    }

    public List<ProgressTracking> RecentProgress
    {
        get => _recentProgress;
        set => SetProperty(ref _recentProgress, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand SaveProgressCommand { get; }
    public ICommand LoadDataCommand { get; }

    public async Task InitializeAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            IsLoading = true;
            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null) return;

            // Load today's progress if exists
            var todayProgress = await _progressService.GetTodayProgressAsync(currentUser.Id);
            if (todayProgress != null)
            {
                TodayProgress = todayProgress;
            }
            else
            {
                TodayProgress = new ProgressTracking 
                { 
                    UserId = currentUser.Id, 
                    RecordDate = DateTime.Today 
                };
            }

            // Load recent progress for the chart
            var recentData = await _progressService.GetUserProgressAsync(currentUser.Id, 
                DateTime.Today.AddDays(-30), DateTime.Today);
            RecentProgress = recentData.ToList();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error loading data: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private bool CanSaveProgress()
    {
        return !IsLoading && TodayProgress.UserId > 0;
    }

    private async Task SaveProgressAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var currentUser = await _authService.GetCurrentUserAsync();
            if (currentUser == null) return;

            TodayProgress.UserId = currentUser.Id;
            TodayProgress.CreatedDate = DateTime.UtcNow;

            await _progressService.LogProgressAsync(TodayProgress);
            
            // Check for achievements
            await _achievementService.CheckAndAwardAchievementsAsync(currentUser.Id);

            // Refresh data
            await LoadDataAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Error saving progress: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }
}