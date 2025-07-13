using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            RoleComboBox.SelectedIndex = 0; // Default to Member
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Hide any previous error messages
            ErrorMessageTextBlock.Visibility = Visibility.Collapsed;
            
            // Basic validation
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                ShowError("Vui lòng nhập tên đăng nhập.");
                UsernameTextBox.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ShowError("Vui lòng nhập mật khẩu.");
                PasswordBox.Focus();
                return;
            }

            if (RoleComboBox.SelectedItem == null)
            {
                ShowError("Vui lòng chọn loại tài khoản.");
                RoleComboBox.Focus();
                return;
            }

            // Show loading
            LoadingPanel.Visibility = Visibility.Visible;
            LoginButton.IsEnabled = false;

            // Simulate authentication (replace with actual authentication logic)
            var selectedRole = ((ComboBoxItem)RoleComboBox.SelectedItem).Tag.ToString();
            
            // For demo purposes, simulate successful login after a brief delay
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                LoadingPanel.Visibility = Visibility.Collapsed;
                LoginButton.IsEnabled = true;

                // Navigate to appropriate main window based on role
                NavigateToMainWindow(selectedRole);
            };
            timer.Start();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Hide();
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tính năng khôi phục mật khẩu sẽ được triển khai sớm.", 
                          "Thông báo", 
                          MessageBoxButton.OK, 
                          MessageBoxImage.Information);
        }

        private void ShowError(string message)
        {
            ErrorMessageTextBlock.Text = message;
            ErrorMessageTextBlock.Visibility = Visibility.Visible;
        }

        private void NavigateToMainWindow(string role)
        {
            // Store user info for the session (replace with proper session management)
            Application.Current.Properties["Username"] = UsernameTextBox.Text;
            Application.Current.Properties["UserRole"] = role;

            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}