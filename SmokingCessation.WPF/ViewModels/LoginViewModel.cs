using CommunityToolkit.Mvvm.Input;
using SmokingCessation.Business.DTOs;
using SmokingCessation.Business.Interfaces;
using SmokingCessation.WPF.Services;
using System.Windows.Input;

namespace SmokingCessation.WPF.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly ICurrentUserService _currentUserService;

        private string _email = string.Empty;
        private string _password = string.Empty;
        private bool _rememberMe;
        private string _errorMessage = string.Empty;

        public LoginViewModel(
            IAuthenticationService authenticationService,
            INavigationService navigationService,
            IDialogService dialogService,
            ICurrentUserService currentUserService)
        {
            _authenticationService = authenticationService;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _currentUserService = currentUserService;
            
            Title = "Sign In - Smoking Cessation";
            
            LoginCommand = new RelayCommand(async () => await LoginAsync(), CanLogin);
            RegisterCommand = new RelayCommand(NavigateToRegister);
            ForgotPasswordCommand = new RelayCommand(async () => await ForgotPasswordAsync());
        }

        public string Email
        {
            get => _email;
            set
            {
                SetProperty(ref _email, value);
                ErrorMessage = string.Empty;
                LoginCommand.NotifyCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                SetProperty(ref _password, value);
                ErrorMessage = string.Empty;
                LoginCommand.NotifyCanExecuteChanged();
            }
        }

        public bool RememberMe
        {
            get => _rememberMe;
            set => SetProperty(ref _rememberMe, value);
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public RelayCommand LoginCommand { get; }
        public RelayCommand RegisterCommand { get; }
        public RelayCommand ForgotPasswordCommand { get; }

        private bool CanLogin()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && !IsBusy;
        }

        private async Task LoginAsync()
        {
            await ExecuteAsync(async () =>
            {
                var loginDto = new LoginDto
                {
                    Email = Email,
                    Password = Password,
                    RememberMe = RememberMe
                };

                var result = await _authenticationService.LoginAsync(loginDto);

                if (result.Success && result.Token != null && result.User != null)
                {
                    _currentUserService.AuthToken = result.Token;
                    _currentUserService.CurrentUser = result.User;
                    
                    _navigationService.NavigateToMainWindow();
                }
                else
                {
                    ErrorMessage = "Invalid email or password. Please try again.";
                }
            });
        }

        private void NavigateToRegister()
        {
            _navigationService.NavigateTo<Views.RegisterWindow>();
        }

        private async Task ForgotPasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                _dialogService.ShowWarning("Please enter your email address first.");
                return;
            }

            var confirmed = _dialogService.ShowConfirmation(
                $"A password reset link will be sent to {Email}. Continue?",
                "Reset Password");

            if (confirmed)
            {
                await ExecuteAsync(async () =>
                {
                    var result = await _authenticationService.ResetPasswordAsync(Email);
                    if (result.Success)
                    {
                        _dialogService.ShowMessage(
                            "If the email address exists in our system, you will receive a password reset link shortly.",
                            "Password Reset");
                    }
                    else
                    {
                        _dialogService.ShowError(result.ErrorMessage ?? "Failed to send password reset email.");
                    }
                });
            }
        }

        protected override Task HandleErrorAsync(Exception exception)
        {
            ErrorMessage = "An error occurred. Please try again.";
            return Task.CompletedTask;
        }
    }
}