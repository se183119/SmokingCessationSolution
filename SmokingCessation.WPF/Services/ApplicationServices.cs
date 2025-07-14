using SmokingCessation.Business.DTOs;
using SmokingCessation.Business.Interfaces;
using System.Windows;

namespace SmokingCessation.WPF.Services
{
    public class NavigationService : INavigationService
    {
        public void NavigateTo<T>() where T : Window
        {
            var window = App.GetService<T>();
            CloseCurrentWindow();
            window.Show();
        }

        public void NavigateTo<T>(object parameter) where T : Window
        {
            var window = App.GetService<T>();
            
            // Set DataContext if the parameter is a ViewModel
            if (parameter != null)
            {
                window.DataContext = parameter;
            }
            
            CloseCurrentWindow();
            window.Show();
        }

        public void NavigateToMainWindow()
        {
            NavigateTo<Views.MainWindow>();
        }

        public void NavigateToLogin()
        {
            NavigateTo<Views.LoginWindow>();
        }

        public void CloseCurrentWindow()
        {
            Application.Current.Windows
                .OfType<Window>()
                .Where(w => w.IsActive)
                .FirstOrDefault()?.Close();
        }

        public void ShowDialog<T>() where T : Window
        {
            var window = App.GetService<T>();
            window.ShowDialog();
        }

        public void ShowDialog<T>(object parameter) where T : Window
        {
            var window = App.GetService<T>();
            
            if (parameter != null)
            {
                window.DataContext = parameter;
            }
            
            window.ShowDialog();
        }
    }

    public class DialogService : IDialogService
    {
        public void ShowMessage(string message, string title = "Information")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowError(string message, string title = "Error")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowWarning(string message, string title = "Warning")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public bool ShowConfirmation(string message, string title = "Confirmation")
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes;
        }

        public string? ShowInput(string message, string title = "Input", string defaultValue = "")
        {
            // For simplicity, using a basic input dialog
            // In a real application, you might want to create a custom dialog
            var input = Microsoft.VisualBasic.Interaction.InputBox(message, title, defaultValue);
            return string.IsNullOrEmpty(input) ? null : input;
        }

        public string? ShowOpenFileDialog(string filter = "All files (*.*)|*.*")
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = filter
            };
            
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public string? ShowSaveFileDialog(string filter = "All files (*.*)|*.*", string defaultFileName = "")
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = filter,
                FileName = defaultFileName
            };
            
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }

    public class CurrentUserService : ICurrentUserService
    {
        private readonly IAuthenticationService _authenticationService;

        public CurrentUserService(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public UserDto? CurrentUser { get; set; }
        public string? AuthToken { get; set; }
        public bool IsLoggedIn => CurrentUser != null && !string.IsNullOrEmpty(AuthToken);

        public void ClearCurrentUser()
        {
            CurrentUser = null;
            AuthToken = null;
        }

        public async Task<bool> RefreshCurrentUserAsync()
        {
            if (string.IsNullOrEmpty(AuthToken))
                return false;

            try
            {
                var user = await _authenticationService.GetCurrentUserAsync(AuthToken);
                if (user != null)
                {
                    CurrentUser = user;
                    return true;
                }
            }
            catch
            {
                // Token might be expired or invalid
            }

            ClearCurrentUser();
            return false;
        }
    }
}