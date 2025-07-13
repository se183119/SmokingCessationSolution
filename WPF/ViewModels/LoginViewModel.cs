using BusinessObjects;
using WPF.Services;

namespace WPF.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IAuthenticationService _authService;
    private readonly INavigationService _navigationService;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading = false;

    public LoginViewModel(IAuthenticationService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        LoginCommand = new RelayCommand(async () => await LoginAsync(), () => CanLogin());
        RegisterCommand = new RelayCommand(NavigateToRegister);
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
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

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

    private bool CanLogin()
    {
        return !string.IsNullOrWhiteSpace(Username) && 
               !string.IsNullOrWhiteSpace(Password) && 
               !IsLoading;
    }

    private async Task LoginAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            var user = await _authService.AuthenticateAsync(Username, Password);
            if (user != null)
            {
                await _authService.SetCurrentUserAsync(user);
                NavigateBasedOnRole(user.Role);
            }
            else
            {
                ErrorMessage = "Invalid username or password.";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Login failed: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void NavigateBasedOnRole(UserRole role)
    {
        switch (role)
        {
            case UserRole.Admin:
                _navigationService.NavigateTo("AdminDashboard");
                break;
            case UserRole.Coach:
                _navigationService.NavigateTo("CoachDashboard");
                break;
            case UserRole.Member:
                _navigationService.NavigateTo("MemberDashboard");
                break;
            default:
                _navigationService.NavigateTo("Home");
                break;
        }
    }

    private void NavigateToRegister()
    {
        _navigationService.NavigateTo("Register");
    }
}