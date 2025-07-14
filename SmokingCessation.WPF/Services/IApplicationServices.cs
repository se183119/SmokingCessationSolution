using System.Windows;

namespace SmokingCessation.WPF.Services
{
    public interface INavigationService
    {
        void NavigateTo<T>() where T : Window;
        void NavigateTo<T>(object parameter) where T : Window;
        void NavigateToMainWindow();
        void NavigateToLogin();
        void CloseCurrentWindow();
        void ShowDialog<T>() where T : Window;
        void ShowDialog<T>(object parameter) where T : Window;
    }

    public interface IDialogService
    {
        void ShowMessage(string message, string title = "Information");
        void ShowError(string message, string title = "Error");
        void ShowWarning(string message, string title = "Warning");
        bool ShowConfirmation(string message, string title = "Confirmation");
        string? ShowInput(string message, string title = "Input", string defaultValue = "");
        string? ShowOpenFileDialog(string filter = "All files (*.*)|*.*");
        string? ShowSaveFileDialog(string filter = "All files (*.*)|*.*", string defaultFileName = "");
    }

    public interface ICurrentUserService
    {
        Business.DTOs.UserDto? CurrentUser { get; set; }
        string? AuthToken { get; set; }
        bool IsLoggedIn { get; }
        void ClearCurrentUser();
        Task<bool> RefreshCurrentUserAsync();
    }
}